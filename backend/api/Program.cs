using FluentValidation;
using MassTransit;
using Serilog;

using TodoChallenge.Api;
using TodoChallenge.Api.Endpoints;
using TodoChallenge.Contracts.Entities;
using TodoChallenge.Contracts.Options;
using TodoChallenge.Contracts.Validators;

var assembly = typeof(Program).Assembly;
Log.Logger = Logger.Build();

try
{
    Log.Information("Starting host");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables("POSTGRES_");
    builder.Configuration.AddEnvironmentVariables("RABBITMQ_");

    var postgresOpts = builder.Configuration.Get<DatabaseOptions>() ?? throw new InvalidOperationException("Postgres options are required");
    var rabbitmqOpts = builder.Configuration.Get<BrokerOptions>() ?? throw new InvalidOperationException("Rabbitmq options are required");

    builder.Services.AddSerilog();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddValidatorsFromAssemblyContaining<Todo>();
    builder.Services.AddAutoMapper(assembly);
    builder.Services.AddMassTransit(cfg =>
    {
        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.AddConsumers(assembly);

        cfg.UsingRabbitMq(
            (context, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.ConfigureEndpoints(context);
                busFactoryConfigurator.Host(rabbitmqOpts.ConnectionString);
            }
        );
    });
    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();
    builder.Services.AddCors();
    builder.Services.AddScoped<IValidator<ITodo>, TodoValidator>();

    var app = builder.Build();

    app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
    app.UseExceptionHandler(b => b.Run(async context => await Results.Problem().ExecuteAsync(context)));
    app.UseSerilogRequestLogging();
    app.MapHealthChecks("/healthz");
    app.MapControllers();
    app.AddTodosEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

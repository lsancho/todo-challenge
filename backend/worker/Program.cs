using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoChallenge.Contracts.Options;
using TodoChallenge.Worker;

var assembly = typeof(Program).Assembly;
Log.Logger = Logger.Build();

try
{
    Log.Information("Starting host");

    var builder = Host.CreateApplicationBuilder(args);

    var postgresOpts =
        builder.Configuration.Get<DatabaseOptions>()
        ?? throw new InvalidOperationException("Postgres options are required");
    var rabbitmqOpts =
        builder.Configuration.Get<BrokerOptions>()
        ?? throw new InvalidOperationException("Rabbitmq options are required");

    builder.Services.AddSerilog();
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(postgresOpts.ConnectionString).UseSnakeCaseNamingConvention()
    );
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
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
    builder.Services.AddHealthChecks();
    builder.Services.AddHostedService<ReadOperationsWorker>();

    var app = builder.Build();

    app.Services.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();

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

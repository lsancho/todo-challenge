namespace Serilog;

public class Logger
{
    public static Serilog.ILogger Build()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", true)
            .Build();

        return new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateBootstrapLogger();
    }
}

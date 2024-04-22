using Microsoft.Extensions.Configuration;

namespace TodoChallenge.Contracts.Options;

public record DatabaseOptions
{
    [ConfigurationKeyName("POSTGRES_HOST")]
    public string? Host { get; set; }

    [ConfigurationKeyName("POSTGRES_PORT")]
    public int Port { get; set; }

    [ConfigurationKeyName("POSTGRES_DB")]
    public string? Database { get; set; }

    [ConfigurationKeyName("POSTGRES_USER")]
    public string? User { get; set; }

    [ConfigurationKeyName("POSTGRES_PASSWORD")]
    public string? Password { get; set; }

    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Database};Username={User};Password={Password}";
}

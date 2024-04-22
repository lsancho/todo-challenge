using Microsoft.Extensions.Configuration;

namespace TodoChallenge.Contracts.Options;

public record BrokerOptions
{
    [ConfigurationKeyName("RABBITMQ_HOST")]
    public string? Host { get; set; }

    [ConfigurationKeyName("RABBITMQ_PORT")]
    public int Port { get; set; }

    [ConfigurationKeyName("RABBITMQ_DB")]
    public string? Database { get; set; }

    [ConfigurationKeyName("RABBITMQ_USER")]
    public string? User { get; set; }

    [ConfigurationKeyName("RABBITMQ_PASSWORD")]
    public string? Password { get; set; }

    [ConfigurationKeyName("RABBITMQ_VHOST")]
    public string? VHost { get; set; }

    public string ConnectionString => $"amqp://{User}:{Password}@{Host}:{Port}{VHost}";
}

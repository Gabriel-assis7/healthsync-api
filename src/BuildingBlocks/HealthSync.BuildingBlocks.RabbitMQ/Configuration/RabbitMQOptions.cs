namespace HealthSync.BuildingBlocks.RabbitMQ.Configuration;

public sealed class RabbitMQOptions
{
    public required string HostName { get; init; }
    public required string VirtualHost { get; init; } = "/";
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public int Port { get; init; } = 5672;
}
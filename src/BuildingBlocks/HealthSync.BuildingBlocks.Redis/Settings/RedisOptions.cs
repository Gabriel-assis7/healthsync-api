using HealthSync.BuildingBlocks.Resiliency.Options;

namespace HealthSync.BuildingBlocks.Redis.Settings;

public class RedisOptions
{
    public bool Enabled { get; set; } = true;
    public string? ClientName { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public bool EnableSsl { get; set; } = true;
    public string? Host { get; set; }
    public int Port { get; set; } = 6379;
    public bool? AbortOnConnectFail { get; set; }
    public int ConnectTimeout { get; set; } = 5000;
    public int ConnectRetry { get; set; } = 3;
    public RetryPolicyOptions? ReconnectRetryPolicy { get; set; }
    public int SyncTimeout { get; set; } = 5000;
    public int AsyncTimeout { get; set; } = 5000;
    public int KeepAlive { get; set; } = 30;
}

namespace HealthSync.BuildingBlocks.RabbitMQ.Options
{
    public class RabbitMqConnectionOptions
    {
        public string HostName { get; set; } = "localhost";

        public int Port { get; set; } = 5672;

        public string UserName { get; set; } = "guest";

        public string Password { get; set; } = "guest";

        public string VirtualHost { get; set; } = "/";

        public bool UseSsl { get; set; }

        public int ConnectionTimeoutMilliseconds { get; set; } = 30000;

        public int RequestedHeartbeatSeconds { get; set; } = 60;
    }
}

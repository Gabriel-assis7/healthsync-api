namespace HealthSync.BuildingBlocks.RabbitMQ.Options
{
    public class RabbitMqQueueOptions
    {
        public string Name { get; set; } = string.Empty;

        public bool Durable { get; set; } = true;

        public bool Exclusive { get; set; }

        public bool AutoDelete { get; set; }

        public IDictionary<string, object> Arguments { get; set; } =
            new Dictionary<string, object>();
    }
}

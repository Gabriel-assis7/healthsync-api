namespace HealthSync.BuildingBlocks.RabbitMQ.Options
{
    public class RabbitMqExchangeOptions
    {
        public string Type { get; set; } = "direct";

        public bool Durable { get; set; } = true;

        public bool AutoDelete { get; set; }

        public string DeadLetterExchange { get; set; } = "default.dlx.exchange";

        public string DeadLetterExchangeType { get; set; } = "direct";

        public bool RequeueFailedMessages { get; set; } = true;

        public int RequeueAttempts { get; set; } = 2;

        public int RequeueTimeoutMilliseconds { get; set; } = 200;

        public bool AutoAcknowledge { get; set; } = false;
    }
}

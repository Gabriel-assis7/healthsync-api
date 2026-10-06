using System.Collections;
using RabbitMQ.Client;

namespace HealthSync.BuildingBlocks.RabbitMQ.Exchange;

public class RabbitExchange(string name)
{
    public string Name { get; private set; } = name;
    public string Type { get; set; } = ExchangeType.Fanout;
    public bool Durable { get; set; } = false;
    public bool AutoDelete { get; set; } = true;
    public required IDictionary Arguments { get; set; }
    public string DeadLetterExchange { get; set; } = "default.dlx.exchange";

    public string DeadLetterExchangeType { get; set; } = "fanout";

    public bool RequeueFailedMessages { get; set; } = true;

    public int RequeueAttempts { get; set; } = 2;

    public int RequeueTimeoutMilliseconds { get; set; } = 200;

    public bool AutoAcknowledge { get; set; } = false;
}
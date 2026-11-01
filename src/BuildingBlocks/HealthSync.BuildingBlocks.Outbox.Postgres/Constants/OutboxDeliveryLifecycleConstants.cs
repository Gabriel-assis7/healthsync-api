namespace HealthSync.BuildingBlocks.Outbox.Postgres.Constants;

internal static class OutboxDeliveryLifecycleConstants
{
    internal const int RedeliveryMaxThreshold = 5;
    internal const int RedeliveryMaxThresholdForBrokerErrors = 10;
    internal const int RedeliveryDelay = 250;
    internal const int RecoveryPeriodAfterInMilliseconds = 60000;
    internal const int PrefetchCount = 1;
    internal const int QueueWaitDuration = 100;
    internal const int RecoveryQueueWaitDuration = 1000;
}

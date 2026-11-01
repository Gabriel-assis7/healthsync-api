using HealthSync.BuildingBlocks.Core.Entities;

namespace HealthSync.BuildingBlocks.Core.Persistence;

public interface IOutboxRepository
{
    Task<OutboxMessage> CreateAsync(OutboxMessage message, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OutboxMessage>> CreateRangeAsync(
        IReadOnlyCollection<OutboxMessage> messages,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OutboxMessage>> GetPendingMessagesAsync(
        int batchSize,
        CancellationToken cancellationToken = default);

    Task<OutboxMessage?> GetByIdAsync(Guid messageId, CancellationToken cancellationToken = default);

    Task<bool> MarkAsPublishedAsync(
        Guid messageId,
        DateTime leaseTimeExpiresAt,
        CancellationToken cancellationToken = default);

    Task<OutboxFailureTransition> MarkAsFailedAsync(
        Guid messageId,
        string error,
        bool isRetryable,
        int retryDelayMilliseconds,
        DateTime failedAt,
        DateTime leaseTimeExpiresAt,
        CancellationToken cancellationToken = default);
}

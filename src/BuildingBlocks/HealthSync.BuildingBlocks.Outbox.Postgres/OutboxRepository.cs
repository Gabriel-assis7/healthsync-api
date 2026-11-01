using HealthSync.BuildingBlocks.Core.Entities;
using HealthSync.BuildingBlocks.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HealthSync.BuildingBlocks.Outbox.Postgres;

public sealed class OutboxRepository<TDbContext>(TDbContext dbContext) : IOutboxRepository
    where TDbContext : DbContext
{
    private readonly DbSet<OutboxMessage> _messages = dbContext.Set<OutboxMessage>();

    public async Task<OutboxMessage> CreateAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        await _messages.AddAsync(message, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return message;
    }

    public async Task<IReadOnlyList<OutboxMessage>> CreateRangeAsync(
        IReadOnlyCollection<OutboxMessage> messages,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        await _messages.AddRangeAsync(messages, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return [.. messages];
    }

    public async Task<IReadOnlyList<OutboxMessage>> GetPendingMessagesAsync(
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchSize);

        var now = DateTime.UtcNow;
        return await _messages
            .AsNoTracking()
            .Where(message =>
                (message.Status == OutboxMessageStatus.Pending
                    && (message.NextRetryAt == null || message.NextRetryAt <= now))
                || ((message.Status == OutboxMessageStatus.Processing
                        || message.Status == OutboxMessageStatus.DeadLettered)
                    && message.NextRetryAt != null
                    && message.NextRetryAt <= now))
            .OrderBy(message => message.CreatedAt)
            .ThenBy(message => message.Id)
            .Take(batchSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<OutboxMessage?> GetByIdAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        return await _messages
            .AsNoTracking()
            .FirstOrDefaultAsync(message => message.Id == messageId, cancellationToken)
            .ConfigureAwait(false);
    }


    public async Task<bool> MarkAsPublishedAsync(
        Guid messageId,
        DateTime leaseTimeExpiresAt,
        CancellationToken cancellationToken = default)
    {
        var message = await _messages
            .Where(m => m.Id == messageId
            && m.Status == OutboxMessageStatus.Processing
            && m.NextRetryAt == leaseTimeExpiresAt)
            .ExecuteUpdateAsync(
                m => m
                    .SetProperty(m => m.Status, OutboxMessageStatus.Published)
                    .SetProperty(m => m.PublishedAt, DateTime.UtcNow),
                cancellationToken)
            .ConfigureAwait(false);

        return message == 1;
    }

    public async Task<OutboxFailureTransition> MarkAsFailedAsync(
        Guid messageId,
        string error,
        bool isRetryable,
        int retryDelayMilliseconds,
        DateTime failedAt,
        DateTime leaseTimeExpiresAt,
        CancellationToken cancellationToken = default)
    {
        var processingMessage = _messages
            .Where(m => m.Id == messageId
            && m.Status == OutboxMessageStatus.Processing
            && m.NextRetryAt == leaseTimeExpiresAt
            );
        var deadLetteredMessage = await processingMessage
            .Where(m => m.RetryCount + 1 >= m.RetryLimit)
            .ExecuteUpdateAsync(
                m => m
                    .SetProperty(m => m.Status, OutboxMessageStatus.DeadLettered)
                    .SetProperty(m => m.DeadLetteredAt, failedAt)
                    .SetProperty(m => m.LastError, error)
                    .SetProperty(m => m.NextRetryAt, leaseTimeExpiresAt),
                cancellationToken)
            .ConfigureAwait(false);

        if (deadLetteredMessage == 1)
        {
            return OutboxFailureTransition.DeadLettered;
        }

        if (!isRetryable)
        {
            var failedMessage = await processingMessage
                .Where(m => m.RetryCount + 1 < m.RetryLimit)
                .ExecuteUpdateAsync(
                    m => m
                        .SetProperty(m => m.Status, OutboxMessageStatus.Failed)
                        .SetProperty(m => m.LastError, error)
                        .SetProperty(m => m.NextRetryAt, (DateTime?)null),
                    cancellationToken)
                .ConfigureAwait(false);

            return failedMessage == 1
                ? OutboxFailureTransition.Failed
                : OutboxFailureTransition.OwnershipLost;
        }

        var scheduledMessage = await processingMessage
            .Where(m => m.RetryCount + 1 < m.RetryLimit)
            .ExecuteUpdateAsync(
                m => m
                    .SetProperty(m => m.Status, OutboxMessageStatus.Pending)
                    .SetProperty(m => m.LastError, error)
                    .SetProperty(m => m.NextRetryAt, failedAt.AddMilliseconds(retryDelayMilliseconds))
                    .SetProperty(m => m.RetryCount, m => m.RetryCount + 1)
                    .SetProperty(m => m.DeadLetteredAt, (DateTime?)null),
                cancellationToken)
            .ConfigureAwait(false);

        return scheduledMessage == 1
            ? OutboxFailureTransition.RetryScheduled
            : OutboxFailureTransition.OwnershipLost;
    }
}

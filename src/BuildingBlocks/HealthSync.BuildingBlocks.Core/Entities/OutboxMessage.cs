using HealthSync.BuildingBlocks.Abstraction.Domain;
using HealthSync.BuildingBlocks.Abstraction.Domain.Entities;

namespace HealthSync.BuildingBlocks.Core.Entities;

public sealed class OutboxMessage : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string AggregateType { get; set; }
    public Guid AggregateId { get; set; }
    public required string EventType { get; set; }
    public string? Payload { get; set; }
    public int RetryLimit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    DateTime IHaveCreator.CreatedAt
    {
        get => CreatedAt;
        init => CreatedAt = value;
    }

    public DateTime? PublishedAt { get; set; }
    public DateTime? NextAttemptAt { get; set; }
    public DateTime? NextRetryAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ClaimToken { get; set; }
    public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;
    public int RetryCount { get; set; }
    public DateTime? DeadLetteredAt { get; set; }
    public string? LastError { get; set; }
}

public enum OutboxMessageStatus
{
    Pending = 1,
    Processing = 2,
    Published = 3,
    Failed = 4,
    DeadLettered = 5
}

public enum OutboxFailureTransition
{
    OwnershipLost,
    RetryScheduled,
    Failed,
    DeadLettered
}

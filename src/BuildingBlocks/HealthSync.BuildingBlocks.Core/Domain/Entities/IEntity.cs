namespace HealthSync.BuildingBlocks.Core.Domain.Entities;

public interface IEntity
{
    Guid Id { get; set; }
    DateTime Created { get; protected init; }
    DateTime? Updated { get; protected set; }
}
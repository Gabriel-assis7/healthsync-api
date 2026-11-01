namespace HealthSync.BuildingBlocks.Abstraction.Domain;

public interface IHaveCreator
{
    DateTime CreatedAt { get; protected init; }
    DateTime? UpdatedAt { get; protected set; }
}
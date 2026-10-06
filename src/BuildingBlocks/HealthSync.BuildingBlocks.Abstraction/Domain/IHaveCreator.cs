namespace HealthSync.BuildingBlocks.Abstraction.Domain;

public interface IHaveCreator
{
    DateTime Created { get; protected init; }
    DateTime? Updated { get; protected set; }
}
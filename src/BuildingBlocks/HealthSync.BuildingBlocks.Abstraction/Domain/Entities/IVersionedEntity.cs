namespace HealthSync.BuildingBlocks.Abstraction.Domain.Entities;

public interface IVersionedEntity
{
    int Version { get; set; }
}
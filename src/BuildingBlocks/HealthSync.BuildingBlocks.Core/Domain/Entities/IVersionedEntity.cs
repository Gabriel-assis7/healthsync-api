namespace HealthSync.BuildingBlocks.Core.Domain.Entities;

public interface IVersionedEntity
{
    int Version { get; set; }
}
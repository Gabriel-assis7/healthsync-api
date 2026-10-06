namespace HealthSync.BuildingBlocks.Abstraction.Domain.Entities;

public interface IAuditable
{
    DateTime CreatedTimestamp { get; set; }
    DateTime UpdatedTimestamp { get; set; }
}
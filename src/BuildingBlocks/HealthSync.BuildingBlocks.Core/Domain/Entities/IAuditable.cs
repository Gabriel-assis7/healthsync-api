namespace HealthSync.BuildingBlocks.Core.Domain.Entities;

public interface IAuditable
{
    DateTime CreatedTimestamp { get; set; }
    DateTime UpdatedTimestamp { get; set; }
}
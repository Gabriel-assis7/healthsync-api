namespace HealthSync.BuildingBlocks.Core.Domain.Entities;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedTimestamp { get; set; }
    void OnDelete();
}
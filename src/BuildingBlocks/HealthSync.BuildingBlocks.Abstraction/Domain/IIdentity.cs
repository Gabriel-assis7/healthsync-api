namespace HealthSync.BuildingBlocks.Abstraction.Domain;

public interface IIdentity<out TId>
{
    public TId Value { get; }
}
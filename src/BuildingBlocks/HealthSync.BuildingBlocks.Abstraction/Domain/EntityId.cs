namespace HealthSync.BuildingBlocks.Abstraction.Domain;

public record EntityId<T> : Identity<T>
{
    protected EntityId(T value)
    {
        Id = value;
    }

    public static implicit operator T(EntityId<T> id)
    {
        ArgumentNullException.ThrowIfNull(id.Id);
        return id.Id;
    }

    // Should be passed to Value Objects to create a new instance of EntityId<T>
    public static EntityId<T> Of(T id)
    {
        return new EntityId<T>(id);
    }
}
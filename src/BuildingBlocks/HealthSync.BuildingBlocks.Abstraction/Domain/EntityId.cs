using HEalthSync.BuildingBlocks.Abstraction.Domain;

namespace HealthSync.BuildingBlocks.Abstraction.Domain;

public record EntityId<T> : Identity<T>
{
    protected EntityId(T value)
    {
        Value = value;
    }

    public static implicit operator T(EntityId<T> id)
    {
        ArgumentNullException.ThrowIfNull(id.Value);
        return id.Value;
    }

    // Should be passed to Value Objects to create a new instance of EntityId<T>
    public static EntityId<T> Of(T id)
    {
        return new EntityId<T>(id);
    }
}
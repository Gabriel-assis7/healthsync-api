namespace HealthSync.BuildingBlocks.Abstraction.Domain;

public abstract record Identity<TId>
{
    public TId Id { get; init; } = default!;

    public static implicit operator TId(Identity<TId> identityId)
    {
        return identityId.Id;
    }
}
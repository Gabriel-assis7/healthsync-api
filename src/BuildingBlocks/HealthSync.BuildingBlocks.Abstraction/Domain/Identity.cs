namespace HEalthSync.BuildingBlocks.Abstraction.Domain;

public abstract record Identity<TId>
{
    public TId Value { get; init; } = default!;

    public static implicit operator TId(Identity<TId> identityId)
    {
        return identityId.Value;
    }
}
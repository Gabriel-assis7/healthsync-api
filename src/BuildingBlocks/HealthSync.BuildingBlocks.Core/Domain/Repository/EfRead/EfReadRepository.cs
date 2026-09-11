using HealthSync.BuildingBlocks.Core.Domain.Aggregate;

namespace HealthSync.BuildingBlocks.Core.Domain.Repository.EfRead;

public abstract class EfReadRepository<TAggregateRoot, TAggregateRootId> : IReadRepository<TAggregateRoot, TAggregateRootId>
    where TAggregateRoot : IAggregateRoot
    where TAggregateRootId : IEquatable<TAggregateRootId>
{
    public int Count()
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync()
    {
        throw new NotImplementedException();
    }

    public TAggregateRoot Get(TAggregateRootId id)
    {
        throw new NotImplementedException();
    }

    public List<TAggregateRoot> GetAllList()
    {
        throw new NotImplementedException();
    }

    public Task<List<TAggregateRoot>> GetAllListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TAggregateRoot> GetAsync(TAggregateRootId id)
    {
        throw new NotImplementedException();
    }

    public long LongCount()
    {
        throw new NotImplementedException();
    }

    public Task<long> LongCountAsync()
    {
        throw new NotImplementedException();
    }
}
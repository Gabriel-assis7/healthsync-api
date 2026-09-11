using HealthSync.BuildingBlocks.Core.Domain.Aggregate;

namespace HealthSync.BuildingBlocks.Core.Domain.Repository.EfRead;

public interface IReadRepository<TAggregateRoot, in TAggregateRootId>
    where TAggregateRoot : IAggregateRoot
    where TAggregateRootId : IEquatable<TAggregateRootId>
{
    List<TAggregateRoot> GetAllList();
    Task<List<TAggregateRoot>> GetAllListAsync();
    TAggregateRoot Get(TAggregateRootId id);
    Task<TAggregateRoot> GetAsync(TAggregateRootId id);
    int Count();
    Task<int> CountAsync();
    long LongCount();
    Task<long> LongCountAsync();
}
using System.Data.Common;

namespace HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;

public interface IConnectionFactory : IDisposable
{
    Task<DbConnection> GetOrCreateConnectionAsync(CancellationToken cancellationToken = default);
}
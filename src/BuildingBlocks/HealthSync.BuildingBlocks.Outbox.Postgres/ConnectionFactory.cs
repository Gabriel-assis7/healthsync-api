using System.Data.Common;
using HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;

namespace HealthSync.BuildingBlocks.Outbox.Postgres;

public class ConnectionFactory(string? connectionString) : IConnectionFactory
{
    private readonly string? _connectionString = connectionString;
    private DbConnection? _dbConnection;

    public async Task<DbConnection> GetOrCreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbConnection is null || _dbConnection.State != System.Data.ConnectionState.Open)
        {
            _dbConnection = new Npgsql.NpgsqlConnection(_connectionString);
            await _dbConnection.OpenAsync(cancellationToken);
        }

        return _dbConnection;
    }

    public void Dispose()
    {
        // We close the connection only if it is open, otherwise we just dispose it.
        if (_dbConnection is { State: System.Data.ConnectionState.Open })
            _dbConnection.Dispose();
        GC.SuppressFinalize(this);
    }
}
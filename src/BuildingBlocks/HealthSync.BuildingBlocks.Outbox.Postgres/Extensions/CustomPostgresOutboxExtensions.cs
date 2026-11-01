using System.Reflection;
using HealthSync.BuildingBlocks.Abstraction.Persistence;
using HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;
using HealthSync.BuildingBlocks.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HealthSync.BuildingBlocks.Outbox.Postgres.Extensions;

public static class CustomPostgresOutboxExtensions
{
    public static IHostApplicationBuilder AddPostgresContextDb<TDbContext>(
        this IHostApplicationBuilder builder,
        string? connectionStringName,
        Action<DbContextOptionsBuilder>? configure = null,
        Action<PostgresOptions>? configurator = null,
        Assembly? migrationAssembly = null)
        where TDbContext : DbContext, IDbFacadeResolver
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var postgresOptions = new PostgresOptions();
        builder.Configuration.GetSection("Postgres").Bind(postgresOptions);
        configurator?.Invoke(postgresOptions);
        builder.Services.AddSingleton(Options.Create(postgresOptions));

        var configuredConnectionString = string.IsNullOrWhiteSpace(connectionStringName)
            ? null
            : builder.Configuration.GetConnectionString(connectionStringName);
        var connectionString = !string.IsNullOrWhiteSpace(configuredConnectionString)
            ? configuredConnectionString
            : postgresOptions.ConnectionString;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Postgres connection string '{connectionStringName}' or `PostgresOptions.ConnectionString` not found."
            );
        }

        builder.Services.AddScoped<IConnectionFactory>(_ => new ConnectionFactory(connectionString));
        builder.Services.AddScoped<IDbFacadeResolver>(provider => provider.GetRequiredService<TDbContext>());

        builder.Services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                sqlOptions =>
                {
                    var assemblyName = migrationAssembly?.GetName().Name
                        ?? typeof(TDbContext).Assembly.GetName().Name;
                    sqlOptions.MigrationsAssembly(assemblyName);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }
            ).UseSnakeCaseNamingConvention();
            configure?.Invoke(options);
        });

        builder.Services.AddScoped<IOutboxRepository, OutboxRepository<TDbContext>>();

        return builder;
    }
}
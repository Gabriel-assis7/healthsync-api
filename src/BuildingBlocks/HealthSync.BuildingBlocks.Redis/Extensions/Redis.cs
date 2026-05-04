using HealthSync.BuildingBlocks.Redis.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace HealthSync.BuildingBlocks.Redis.Extensions;

public static class AddRedisExtensions
{
    public static IServiceCollection AddRedisMultiplexer(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var redisOptions =
            configuration.GetSection("Redis").Get<RedisOptions>() ?? new RedisOptions();

        if (!redisOptions.Enabled)
        {
            return services;
        }

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { $"{redisOptions.Host}:{redisOptions.Port}" },
                User = redisOptions.User,
                Password = redisOptions.Password,
                Ssl = redisOptions.EnableSsl,
                AbortOnConnectFail = redisOptions.AbortOnConnectFail ?? true,
                ConnectTimeout = redisOptions.ConnectTimeout,
                ConnectRetry = redisOptions.ConnectRetry,
                SyncTimeout = redisOptions.SyncTimeout,
                AsyncTimeout = redisOptions.AsyncTimeout,
                ClientName = redisOptions.ClientName,
                KeepAlive = redisOptions.KeepAlive,
            };

            var lazilyConnection = new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(options)
            );

            var connection = lazilyConnection.IsValueCreated
                ? lazilyConnection.Value
                : ConnectionMultiplexer.Connect(options);

            return connection;
        });

        return services;
    }
}

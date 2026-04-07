using HealthSync.BuildingBlocks.MongoDB.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HealthSync.BuildingBlocks.MongoDB.Extensions
{
    public static class MongoDbApplicationExtensions
    {
        public static IHostApplicationBuilder AddMongoDbApplication(
            this IHostApplicationBuilder builder,
            Action<MongoSettings>? configurator = null
        )
        {
            builder
                .Services.AddOptions<MongoSettings>()
                .Bind(builder.Configuration.GetSection("MongoSettings"))
                .Validate(config =>
                {
                    if (!config.Enabled)
                        throw new InvalidOperationException(
                            "MongoDB is disabled in the configuration."
                        );
                    return true;
                })
                .ValidateOnStart();

            if (configurator != null)
            {
                builder.Services.Configure(configurator);
            }

            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var settings = MongoClientSettings.FromConnectionString(config.ConnectionString);

                settings.ConnectTimeout = TimeSpan.FromSeconds(config.ConnectTimeoutSeconds);
                settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(
                    config.MaxConnectionIdleTimeSeconds
                );
                settings.MaxConnectionPoolSize = config.MaxPoolSize;
                settings.MinConnectionPoolSize = config.MinPoolSize;

                return new MongoClient(settings);
            });

            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();

                return client.GetDatabase(config.DatabaseName);
            });

            return builder;
        }
    }
}

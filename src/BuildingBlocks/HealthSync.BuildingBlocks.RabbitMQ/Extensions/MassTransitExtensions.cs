using HealthSync.BuildingBlocks.RabbitMQ.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace HealthSync.BuildingBlocks.RabbitMQ.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddHealthSyncMassTransitWithRabbitMQ(
        this IServiceCollection services,
        Action<RabbitMQOptions> configure
    )
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = new RabbitMQOptions;
                configure(options);

                cfg.Host(options.HostName, options.VirtualHost, h =>
                {
                    h.Username(options.UserName);
                    h.Password(options.Password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}
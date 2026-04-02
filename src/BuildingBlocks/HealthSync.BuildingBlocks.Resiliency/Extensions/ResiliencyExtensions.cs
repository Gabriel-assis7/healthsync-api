using HealthSync.BuildingBlocks.Resiliency.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;

namespace HealthSync.BuildingBlocks.Resiliency.Extensions;

public static class ResiliencyExtensions
{
    public static IHostApplicationBuilder AddCustomHttpResilience(
        this IHostApplicationBuilder builder,
        bool globalHttpResiliency = true
    )
    {
        if (globalHttpResiliency)
        {
            builder.Services.ConfigureHttpClientDefaults(options =>
            {
                options
                    .AddStandardResilienceHandler()
                    .Configure(
                        (cfg, context) =>
                        {
                            var policyOptions = context
                                .GetRequiredService<IOptions<PolicyResiliencyOptions>>()
                                .Value;

                            cfg.AttemptTimeout =
                                policyOptions.Timeouts.ToHttpTimeoutStrategyOptions();

                            cfg.Retry = policyOptions.Retry.ToHttpRetryStrategyOptions();

                            cfg.CircuitBreaker =
                                policyOptions.CircuitBreaker.ToHttpCircuitBreakerStrategyOptions();
                        }
                    );
            });
        }

        return builder;
    }
}

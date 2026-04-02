using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Http.Resilience;

namespace HealthSync.BuildingBlocks.Resiliency.Options;

public class RetryPolicyOptions
{
    [Range(0, 10)]
    public int MaxRetryAttempts { get; set; } = 3;
    public bool UseExponentialBackoff { get; set; } = true;
    public bool UseJitter { get; set; } = true;

    public TimeSpan BaseDelay { get; set; } = TimeSpan.FromSeconds(2);

    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(30);

    public HttpRetryStrategyOptions ToHttpRetryStrategyOptions()
    {
        return new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = MaxRetryAttempts,
            Delay = BaseDelay,
            MaxDelay = MaxDelay,
            UseJitter = UseJitter,
        };
    }

    public void ShowOptions()
    {
        Console.WriteLine("=== Retry Policy Options ===");
        Console.WriteLine("Max Retry Attempts: {0}", MaxRetryAttempts);
        /* Console.WriteLine("Use Exponential Backoff: {0}", UseExponentialBackoff); */
        Console.WriteLine("Use Jitter: {0}", UseJitter);
        Console.WriteLine("Base Delay: {0}", BaseDelay);
        Console.WriteLine("Max Delay: {0}", MaxDelay);
        Console.WriteLine("============================");
    }
}

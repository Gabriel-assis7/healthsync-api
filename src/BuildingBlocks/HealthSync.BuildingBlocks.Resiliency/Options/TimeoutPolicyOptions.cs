using Microsoft.Extensions.Http.Resilience;

namespace HealthSync.BuildingBlocks.Resiliency.Options;

public class TimeoutPolicyOptions
{
    public int TimeOutInSeconds { get; set; } = 30;

    public HttpTimeoutStrategyOptions ToHttpTimeoutStrategyOptions()
    {
        return new HttpTimeoutStrategyOptions { Timeout = TimeSpan.FromSeconds(TimeOutInSeconds) };
    }

    public void ShowOptions()
    {
        Console.WriteLine("=== Timeout Policy Options ===");
        Console.WriteLine("Timeout (seconds): {0}", TimeOutInSeconds);
        Console.WriteLine("============================");
    }
}

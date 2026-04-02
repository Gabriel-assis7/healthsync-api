using Microsoft.Extensions.Http.Resilience;

namespace HealthSync.BuildingBlocks.Resiliency.Options;

public class CircuitBreakerPolicyOptions
{
    public double FailureRatio { get; set; } = 0.1;

    public int MinimumThroughput { get; set; } = 100;

    public int SamplingDurationSeconds { get; set; } = 30;

    public int BreakDurationSeconds { get; set; } = 5;

    /*     public bool EnableManualControl { get; set; } = false;
     */
    /*     public bool EnableStateProvider { get; set; } = false;
     */
    public HttpCircuitBreakerStrategyOptions ToHttpCircuitBreakerStrategyOptions()
    {
        return new HttpCircuitBreakerStrategyOptions
        {
            BreakDuration = TimeSpan.FromSeconds(BreakDurationSeconds),
            FailureRatio = FailureRatio,
            MinimumThroughput = MinimumThroughput,
            SamplingDuration = TimeSpan.FromSeconds(SamplingDurationSeconds),
        };
    }

    public void ShowOptions()
    {
        Console.WriteLine("=== Circuit Breaker Policy Options ===");
        Console.WriteLine("Failure Ratio: {0}", FailureRatio);
        Console.WriteLine("Minimum Throughput: {0}", MinimumThroughput);
        Console.WriteLine("Sampling Duration (seconds): {0}", SamplingDurationSeconds);
        Console.WriteLine("Break Duration (seconds): {0}", BreakDurationSeconds);
        Console.WriteLine("============================");
    }
}

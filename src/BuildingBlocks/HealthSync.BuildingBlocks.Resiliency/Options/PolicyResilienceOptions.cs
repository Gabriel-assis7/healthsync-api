namespace HealthSync.BuildingBlocks.Resiliency.Options;

public class PolicyResiliencyOptions
{
    public CircuitBreakerPolicyOptions CircuitBreaker { get; set; } = new();
    public RetryPolicyOptions Retry { get; set; } = new();
    public TimeoutPolicyOptions Timeouts { get; set; } = new();
}

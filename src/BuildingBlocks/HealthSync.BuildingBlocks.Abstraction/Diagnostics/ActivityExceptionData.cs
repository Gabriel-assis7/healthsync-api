namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics;

public class ActivityExceptionData : ActivityData
{
    public required Exception Exception { get; set; }
}
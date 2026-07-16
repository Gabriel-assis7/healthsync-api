using System.Diagnostics;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Constants;

namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics
{
    public abstract class DiagnosticPipelineBase
    {
        protected static DiagnosticSource _logger = new DiagnosticListener(DiagnosticListenerConstants.ListenerName);
    }
}
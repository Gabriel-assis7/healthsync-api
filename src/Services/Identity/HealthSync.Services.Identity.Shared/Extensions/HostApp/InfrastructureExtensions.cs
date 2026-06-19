using HealthSync.Services.Identity.Shared.Extensions.WebApp;
using Microsoft.Extensions.Hosting;

namespace HealthSync.Services.Identity.Shared.Extensions.HostApp;

public static partial class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultCors();
        
        return builder;
    }
}
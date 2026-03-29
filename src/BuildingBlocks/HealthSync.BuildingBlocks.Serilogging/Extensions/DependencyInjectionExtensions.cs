using Elastic.Ingest.Elasticsearch;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HealthSync.BuildingBlocks.Serilogging.Extensions;

public static class DependencyInjectionExtensions
{
    public static IHostApplicationBuilder AddCustomSerilog(
        this IHostApplicationBuilder builder,
        Action<LoggerConfiguration>? extraConfigure = null,
        Action<SerilogOptions>? configurator = null
    )
    {
        var serilogOptions =
            builder.Configuration.GetSection("Serilog").Get<SerilogOptions>()
            ?? new SerilogOptions();

        configurator?.Invoke(serilogOptions);

        builder.Logging.ClearProviders();

        builder.Services.AddSerilog(
            (services, lc) =>
            {
                lc.ReadFrom.Configuration(builder.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();

                if (!string.IsNullOrEmpty(serilogOptions.ElasticSearchUrl))
                {
                    lc.WriteTo.Elasticsearch(
                        [new Uri(serilogOptions.ElasticSearchUrl)],
                        opts =>
                        {
                            opts.BootstrapMethod = BootstrapMethod.Failure;
                        }
                    );
                }

                if (!string.IsNullOrEmpty(serilogOptions.SeqUrl))
                {
                    lc.WriteTo.Seq(serilogOptions.SeqUrl);
                }

                extraConfigure?.Invoke(lc);
            }
        );

        return builder;
    }
}

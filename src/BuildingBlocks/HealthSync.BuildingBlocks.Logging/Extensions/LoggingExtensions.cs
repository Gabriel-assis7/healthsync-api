using Elastic.Ingest.Elasticsearch;
using Elastic.Serilog.Sinks;
using HealthSync.BuildingBlocks.Logging.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HealthSync.BuildingBlocks.Logging.Extensions;

public static class LoggingExtensions
{
    public static IHostApplicationBuilder AddCustomSerilog(
        this IHostApplicationBuilder builder,
        Action<LoggerConfiguration>? extraConfigure = null,
        Action<LoggingOptions>? configurator = null
    )
    {
        var serilogOptions =
            builder.Configuration.GetSection("Serilog").Get<LoggingOptions>()
            ?? new LoggingOptions();

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

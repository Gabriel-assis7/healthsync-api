using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using HealthSync.Services.Identity.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HealthSync.Services.Identity.Shared.Extensions.WebApp;

public static class CorsExtensions
{
    private const string AllowCustomCors = "AllowCustomPolicy";

    public static IHostApplicationBuilder AddDefaultCors(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var corsOptions = builder.Configuration.GetSection("Cors").Get<CorsOptions>();
                    var origins = corsOptions?.AllowedOrigins ?? [];

                    policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
        else
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowCustomCors, policy =>
                {
                    var corsOptions = builder.Configuration.GetSection("Cors").Get<CorsOptions>();
                    var origins = corsOptions?.AllowedOrigins ?? [];

                    policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        return builder;
    }

    public static void UseDefaultCors(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.UseCors();
        }
        else
        {
            app.UseCors(AllowCustomCors);
        }
    }
}
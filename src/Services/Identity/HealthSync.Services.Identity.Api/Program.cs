using HealthSync.Services.Identity.Shared.Extensions.HostApp;
using HealthSync.Services.Identity.Shared.Extensions.WebApp;

namespace HealthSync.Services.Identity.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddInfrastructure();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseForwardedHeaders();

        app.UseInfrastructure();

        app.Run();
    }
}
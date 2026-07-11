using HealthSync.BuildingBlocks.Abstraction.Behaviors;
using HealthSync.BuildingBlocks.Abstraction.Cqrs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPipelineBehaviors();
builder.Services.AddTransient<IQueryHandler<SampleQuery, string>, SampleQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/diagnostics", async (IPipelineInvoker invoker, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
{
    var query = new SampleQuery();

    return await invoker.InvokeAsync<SampleQuery, string>(
        query,
        () =>
        {
            var handler = serviceProvider.GetRequiredService<IQueryHandler<SampleQuery, string>>();
            return handler.HandleAsync(query, cancellationToken);
        },
        cancellationToken);
})
.WithName("GetDiagnostics")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public sealed record SampleQuery : IQuery<string>;

public sealed class SampleQueryHandler : IQueryHandler<SampleQuery, string>
{
    public Task<string> HandleAsync(SampleQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        return Task.FromResult("Minimal API pipeline executed successfully.");
    }
}

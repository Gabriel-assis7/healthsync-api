using HealthSync.BuildingBlocks.RabbitMQ.Consumers.Appointments.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace HealthSync.BuildingBlocks.RabbitMQ.Consumers.Appointments;

public class AppointmentCreatedHandler(ILogger<AppointmentCreatedHandler> logger) : IConsumer<AppointmentCreatedCommand>
{
    readonly ILogger<AppointmentCreatedHandler> _logger = logger;

    public async Task Consume(ConsumeContext<AppointmentCreatedCommand> context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var message = context.Message;

        _logger.LogInformation("Received AppointmentCreated event: {AppointmentId}", message.AppointmentId);

        await context.Publish(new AppointmentReady
        {
            AppointmentId = message.AppointmentId,
            PatientId = message.PatientId,
            DoctorId = message.DoctorId,
            ReadyAt = DateTimeOffset.UtcNow
        });
    }
}
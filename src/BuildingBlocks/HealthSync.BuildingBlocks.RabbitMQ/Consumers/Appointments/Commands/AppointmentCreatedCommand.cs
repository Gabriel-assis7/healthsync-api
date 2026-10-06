namespace HealthSync.BuildingBlocks.RabbitMQ.Consumers.Appointments.Commands;

public record AppointmentCreatedCommand
{
    public Guid AppointmentId { get; init; }
    public Guid PatientId { get; init; }
    public Guid DoctorId { get; init; }
    public DateTimeOffset ScheduledAt { get; init; }
}
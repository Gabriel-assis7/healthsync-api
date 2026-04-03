namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Domain
{
    [Serializable]
    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string? message)
            : base(message) { }

        public EntityNotFoundException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public EntityNotFoundException(Type entityType, object? entityId)
            : base($"{entityType?.Name} with ID '{entityId}' was not found.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public EntityNotFoundException(Type entityType, object? entityId, string? message)
            : base(message)
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public EntityNotFoundException(
            Type entityType,
            object? entityId,
            string? message,
            Exception? innerException
        )
            : base(message, innerException)
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public Type? EntityType { get; private set; }

        public object? EntityId { get; private set; }
    }
}

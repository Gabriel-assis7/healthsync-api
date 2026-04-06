using System;

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Domain
{
    [Serializable]
    public class EntityAlreadyExistsException : DomainException
    {
        public EntityAlreadyExistsException() { }

        public EntityAlreadyExistsException(string? message)
            : base(message) { }

        public EntityAlreadyExistsException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public EntityAlreadyExistsException(Type entityType, object? entityId)
            : base($"{entityType?.Name} with ID '{entityId}' already exists.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public EntityAlreadyExistsException(Type entityType, object? entityId, string? message)
            : base(message)
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public Type? EntityType { get; private set; }

        public object? EntityId { get; private set; }
    }
}

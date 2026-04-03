using System;

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Domain
{
    /// <summary>
    /// Thrown when attempting to create an entity that already exists.
    /// Typically results in HTTP 409 Conflict responses.
    /// </summary>
    [Serializable]
    public class EntityAlreadyExistsException : DomainException
    {
        public EntityAlreadyExistsException() { }

        public EntityAlreadyExistsException(string? message)
            : base(message) { }

        public EntityAlreadyExistsException(string? message, Exception? innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Creates an EntityAlreadyExistsException with entity type and identifier context.
        /// </summary>
        public EntityAlreadyExistsException(Type entityType, object? entityId)
            : base($"{entityType?.Name} with ID '{entityId}' already exists.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        /// <summary>
        /// Creates an EntityAlreadyExistsException with entity type, identifier, and custom message.
        /// </summary>
        public EntityAlreadyExistsException(Type entityType, object? entityId, string? message)
            : base(message)
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        /// <summary>
        /// Creates an EntityAlreadyExistsException with entity type, identifier, and conflict property.
        /// </summary>
        public EntityAlreadyExistsException(Type entityType, object? entityId, string? conflictProperty, string? message)
            : base(message)
        {
            EntityType = entityType;
            EntityId = entityId;
            ConflictProperty = conflictProperty;
        }

        /// <summary>
        /// The type of entity that already exists.
        /// </summary>
        public Type? EntityType { get; private set; }

        /// <summary>
        /// The identifier of the entity that already exists.
        /// </summary>
        public object? EntityId { get; private set; }

        /// <summary>
        /// The property causing the conflict (e.g., "Email", "Username"), if applicable.
        /// </summary>
        public string? ConflictProperty { get; private set; }
    }
}

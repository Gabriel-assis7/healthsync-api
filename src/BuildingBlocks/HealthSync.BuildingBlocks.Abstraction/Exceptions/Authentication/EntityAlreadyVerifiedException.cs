using System;

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication
{
    [Serializable]
    public class EntityAlreadyVerifiedException : BaseMessageException
    {
        public EntityAlreadyVerifiedException() { }

        public EntityAlreadyVerifiedException(string? message)
            : base(message) { }
        public EntityAlreadyVerifiedException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public EntityAlreadyVerifiedException(string? entityType, string? message)
            : base($"Already verified {entityType}: {message}")
        {
            EntityType = entityType;
        }
        public EntityAlreadyVerifiedException(string? entityType, string? message, Exception? innerException)
            : base($"Already verified {entityType}: {message}", innerException)
        {
            EntityType = entityType;
        }
        public string? EntityType { get; private set; }
    }
}

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication
{
    [Serializable]
    public class LockedOutException : BaseMessageException
    {
        public LockedOutException()
            : base("User account is locked out.") { }

        public LockedOutException(string? message)
            : base(message) { }
        public LockedOutException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}

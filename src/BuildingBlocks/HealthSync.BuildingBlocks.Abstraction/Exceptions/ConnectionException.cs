namespace HealthSync.BuildingBlocks.Abstraction.Exceptions
{
    [Serializable]
    public class ConnectionException : BaseMessageException
    {
        public ConnectionException() { }

        public ConnectionException(bool isTransient)
        {
            IsTransient = isTransient;
        }

        public ConnectionException(string? message, bool isTransient = false)
            : base(message)
        {
            IsTransient = isTransient;
        }

        public ConnectionException(
            string? messaege,
            Exception? innerException,
            bool isTransient = false
        )
            : base(messaege, innerException)
        {
            IsTransient = isTransient;
        }

        public bool IsTransient { get; }
    }
}

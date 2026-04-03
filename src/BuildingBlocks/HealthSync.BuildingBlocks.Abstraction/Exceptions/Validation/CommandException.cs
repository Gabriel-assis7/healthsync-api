namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Validation
{
    [Serializable]
    public class CommandException : BaseMessageException
    {
        public CommandException(Type commandType, string? message = null)
            : base(message)
        {
            CommandType = commandType;
        }

        public CommandException(Type commandType, string? message, Exception innerException)
            : base(message, innerException)
        {
            CommandType = commandType;
        }

        public CommandException() { }

        public Type? CommandType { get; private set; }
    }
}

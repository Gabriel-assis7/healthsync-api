namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Domain
{
    [Serializable]
    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException() { }

        public BusinessRuleException(string? message)
            : base(message) { }

        public BusinessRuleException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public BusinessRuleException(string? ruleName, string? message)
            : base($"Business rule '{ruleName}' violated: {message}")
        {
            RuleName = ruleName;
        }

        public BusinessRuleException(string? ruleName, Type entityType, string? message)
            : base($"Business rule '{ruleName}' violated for {entityType?.Name}: {message}")
        {
            RuleName = ruleName;
            EntityType = entityType;
        }

        public BusinessRuleException(
            string? ruleName,
            Type entityType,
            string? message,
            Exception? innerException
        )
            : base(
                $"Business rule '{ruleName}' violated for {entityType?.Name}: {message}",
                innerException
            )
        {
            RuleName = ruleName;
            EntityType = entityType;
        }

        public string? RuleName { get; private set; }

        public Type? EntityType { get; private set; }
    }
}

using HealthSync.BuildingBlocks.Abstraction.Exceptions;
using RabbitMQ.Client.Exceptions;

namespace HealthSync.BuildingBlocks.Transport.Messaging.RabbitMQ.Exceptions
{
    [Serializable]
    public class RabbitMqConnectionException : ConnectionException
    {
        public RabbitMqConnectionException() { }

        public RabbitMqConnectionException(string message)
            : base(message) { }

        public RabbitMqConnectionException(string message, Exception innerException)
            : base(message, innerException, IsExceptionTransient(innerException)) { }

        static bool IsExceptionTransient(Exception exception)
        {
            return exception switch
            {
                BrokerUnreachableException bue => bue.InnerException switch
                {
                    AuthenticationFailureException => false,
                    _ => true,
                },
                _ => true,
            };
        }
    }
}

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions
{
    using System;

    [Serializable]
    public class EndpointException : AbstractUriException
    {
        public EndpointException() { }

        public EndpointException(Uri uri)
            : base(uri) { }

        public EndpointException(Uri uri, string message)
            : base(uri, message) { }

        public EndpointException(Uri uri, string message, Exception innerException)
            : base(uri, message, innerException) { }
    }
}

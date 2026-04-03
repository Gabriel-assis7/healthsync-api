using System;

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions
{
    [Serializable]
    public class AbstractUriException : BaseMessageException
    {
        public AbstractUriException() { }

        public AbstractUriException(Uri uri)
        {
            Uri = uri;
        }

        public AbstractUriException(Uri uri, string? message)
            : base($"{uri} => {message}")
        {
            Uri = uri;
        }

        public AbstractUriException(Uri uri, string? message, Exception? innerException)
            : base($"{uri} => {message}, innerException: {innerException?.Message}", innerException)
        {
            Uri = uri;
        }

        public Uri? Uri { get; protected set; }
    }
}

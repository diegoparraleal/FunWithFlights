using System.Net;

namespace FunWithFlights.Infrastructure.ExternalProviders;

internal class KnownException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public KnownException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
}
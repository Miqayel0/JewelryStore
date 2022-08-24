using System.Net;

namespace Common.Exceptions;

public class ForbiddenAccessException : HttpRequestException
{
    public ForbiddenAccessException()
        : base(HttpStatusCode.Forbidden)
    {
    }

    public ForbiddenAccessException(string message)
        : base(message, HttpStatusCode.Forbidden)
    {
    }
}

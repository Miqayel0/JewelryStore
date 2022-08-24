using System.Net;

namespace Common.Exceptions;

public class BadRequestException : HttpRequestException
{
    public BadRequestException()
        : base(HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(string message)
        : base(message, HttpStatusCode.BadRequest)
    {
    }
}

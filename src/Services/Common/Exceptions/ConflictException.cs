using System.Net;

namespace Common.Exceptions;

public class ConflictException : HttpRequestException
{
    
    public ConflictException()
        : base(HttpStatusCode.Conflict)
    {
    }

    public ConflictException(string message)
        : base(message, HttpStatusCode.Conflict)
    {
    }
}

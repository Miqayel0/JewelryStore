using System.Net;

namespace Common.Exceptions;

public class UnauthorizedException : HttpRequestException
{
    public UnauthorizedException(string message = "Invalid authentication credentials for the target resource.") : base(message, HttpStatusCode.Unauthorized) { }
}

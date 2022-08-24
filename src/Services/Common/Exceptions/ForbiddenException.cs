using System.Net;

namespace Common.Exceptions;

public class ForbiddenException : HttpRequestException
{
    public ForbiddenException(string message = "Do not have permissions for the operation.") : base(message, HttpStatusCode.Forbidden) { }
}

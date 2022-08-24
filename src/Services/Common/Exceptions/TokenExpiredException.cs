using System.Net;

namespace Common.Exceptions;

public class TokenExpiredException : HttpRequestException
{
    public TokenExpiredException(string message = "Token has expired!") : base(message, HttpStatusCode.Unauthorized) { }
}

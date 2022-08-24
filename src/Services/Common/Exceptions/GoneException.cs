using System.Net;

namespace Common.Exceptions;

public class GoneException : HttpRequestException
{
    public GoneException(string message = "Resource is no longer available.") : base(message, HttpStatusCode.Gone) { }
}

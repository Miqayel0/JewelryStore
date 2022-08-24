using System.Net;

namespace Common.Exceptions;

public class HttpRequestException : ApplicationException
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;

    public HttpRequestException()
    {
    }

    public HttpRequestException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
    
    public HttpRequestException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}

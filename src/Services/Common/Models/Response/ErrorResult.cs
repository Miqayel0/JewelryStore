using System.Net;

namespace Common.Models.Response;

public class ErrorResult : UseCaseResult
{
    public ErrorResult(string errorMessage, int statusCode = (int)HttpStatusCode.BadRequest) : base(statusCode)
    {
        ErrorMessage = errorMessage;
    }
 
    public string ErrorMessage { get; }
    public string Error { get; }
}

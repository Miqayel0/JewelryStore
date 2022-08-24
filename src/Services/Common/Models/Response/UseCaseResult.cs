using Microsoft.AspNetCore.Mvc;

namespace Common.Models.Response;

public class UseCaseResult : StatusCodeResult
{

    public UseCaseResult(int statusCode) : base(statusCode) {}

}

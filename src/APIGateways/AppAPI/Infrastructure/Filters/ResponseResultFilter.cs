using System.Collections;
using System.Net;
using Common.Extensions;
using Common.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAPI.Infrastructure.Filters;

public class ResponseResultFilter : IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        var result = context.Result;
        if (!(result is JsonResult || result is ObjectResult))
        {
            return;
        }

        if (result is EmptyResult)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var propInfo = result.GetType().GetProperty("Value");
        var value = propInfo?.GetValue(result);
        if (value == null)
        {
            context.Result = new NotFoundResult();
            return;
        }
        else if (result is BadRequestObjectResult bad && bad.Value is ValidationProblemDetails validation)
        {
            var error = validation.Errors.First();
            value = $"{error.Key}: {error.Value?[0]}";
            value = new ErrorResult(value.ToString(), 400);
            bad.StatusCode = 400;
        }
        else if (value is not UseCaseResult)
        {
            //var statusCode = value.GetPropertyValue("Status") ?? HttpStatusCode.OK;
            value = new DataResult<object>(value);
        }

        propInfo?.SetValue(context.Result, value);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}

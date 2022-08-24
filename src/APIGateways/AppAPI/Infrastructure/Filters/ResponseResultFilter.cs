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

        if (value is ICollection list && list.Count == 0)
        {
            context.Result = new NotFoundResult();
            return;
        }

        if (value is not UseCaseResult)
        {
            var statusCode = value.GetPropertyValue("Status") ?? HttpStatusCode.OK;
            value = new DataResult<object>(value, (int) statusCode);
        }

        propInfo?.SetValue(context.Result, value);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}

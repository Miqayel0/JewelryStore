using Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAPI.Infrastructure.Filters;

public class ValidateModelActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            throw new ValidationException(context.ModelState);
        }
    }


    public void OnActionExecuted(ActionExecutedContext context)
    {
        // var result = context.Result;
        if (context.Canceled == true)
        {
        }

        if (context.Exception != null)
        {
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EMS.Core.Models;
using EMS.Core.Helpers;

namespace EMS.API.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var apiResult = ApiResultFactory.CreateErrorResult(ErrorCode.VALIDATION_ERROR,
                ErrorMessage.VALIDATION_ERROR, errors);

            context.Result = new BadRequestObjectResult(apiResult);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do nothing after action execution
    }
}

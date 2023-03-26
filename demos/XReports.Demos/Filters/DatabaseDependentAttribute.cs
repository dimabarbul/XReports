using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XReports.Demos.Data;

namespace XReports.Demos.Filters;

public sealed class DatabaseDependentAttribute : ActionFilterAttribute
{
    private const string ViewName = "DatabaseNotReady";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!DatabaseSeeder.SeedFinished)
        {
            context.Result = new ViewResult()
            {
                ViewName = ViewName,
            };
        }
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
    }
}

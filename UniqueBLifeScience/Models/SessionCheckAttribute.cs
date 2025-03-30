using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var userName = session.GetString("UserName");

        if (string.IsNullOrEmpty(userName))
        {
            context.Result = new RedirectToActionResult("Login", "Home", null);
        }

        base.OnActionExecuting(context);
    }
}

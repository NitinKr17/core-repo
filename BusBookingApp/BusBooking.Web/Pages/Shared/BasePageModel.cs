using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusBooking.Web.Pages.Shared;

public abstract class BasePageModel : PageModel
{
    public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
    {
        var token = context.HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToPageResult("/Login");
        }

        base.OnPageHandlerExecuting(context);
    }
}

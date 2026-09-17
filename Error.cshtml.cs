using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SpaFlow.Web.Pages;
public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
{
    public void OnGet()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (feature?.Error is not null) logger.LogError(feature.Error, "Unhandled error at {Path}", feature.Path);
    }
}

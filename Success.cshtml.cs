using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SpaFlow.Web.Pages.Booking;
public class SuccessModel : PageModel
{
    [BindProperty(SupportsGet = true)] public int Id { get; set; }
    public void OnGet() { }
}

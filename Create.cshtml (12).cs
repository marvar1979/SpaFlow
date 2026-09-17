using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Clients;

public class CreateModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    [BindProperty] public Client Client { get; set; } = new();
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Client.Appointments");
        if (!ModelState.IsValid) return Page();
        Client.SpaBusinessId = tenant.SpaBusinessId ?? 0;
        db.Clients.Add(Client);
        await db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}

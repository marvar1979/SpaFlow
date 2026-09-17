using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Settings;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    [BindProperty] public SpaBusiness Business { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync()
    {
        Business = await db.SpaBusinesses.FirstAsync(x => x.Id == (tenant.SpaBusinessId ?? 0));
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var spaId = tenant.SpaBusinessId ?? 0;
        if (Business.Id != spaId) return Forbid();
        ModelState.Remove("Business.Branches");
        if (!ModelState.IsValid) return Page();
        var entity = await db.SpaBusinesses.FirstAsync(x => x.Id == spaId);
        entity.Name = Business.Name; entity.LegalName = Business.LegalName; entity.Phone = Business.Phone; entity.Email = Business.Email; entity.Address = Business.Address; entity.Currency = Business.Currency; entity.TimeZoneId = Business.TimeZoneId;
        await db.SaveChangesAsync();
        return RedirectToPage();
    }
}

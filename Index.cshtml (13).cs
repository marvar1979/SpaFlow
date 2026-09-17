using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Clients;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    public List<Client> Clients { get; set; } = [];
    public async Task OnGetAsync()
    {
        var spaId = tenant.SpaBusinessId ?? 0;
        var query = db.Clients.AsNoTracking().Where(x => x.SpaBusinessId == spaId && x.IsActive);
        if (!string.IsNullOrWhiteSpace(Search))
            query = query.Where(x => x.FullName.Contains(Search) || (x.Phone != null && x.Phone.Contains(Search)) || (x.Email != null && x.Email.Contains(Search)));
        Clients = await query.OrderBy(x => x.FullName).Take(300).ToListAsync();
    }
}

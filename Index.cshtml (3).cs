using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Services;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    public List<SpaService> Services { get; set; } = [];
    public async Task OnGetAsync() => Services = await db.SpaServices.AsNoTracking().Include(x => x.Category).Where(x => x.SpaBusinessId == (tenant.SpaBusinessId ?? 0) && x.IsActive).OrderBy(x => x.Category.DisplayOrder).ThenBy(x => x.Name).ToListAsync();
}

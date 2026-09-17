using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Inventory;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    public List<Product> Products { get; set; } = [];
    public async Task OnGetAsync() => Products = await db.Products.AsNoTracking().Include(x => x.Category).Where(x => x.SpaBusinessId == (tenant.SpaBusinessId ?? 0) && x.IsActive).OrderBy(x => x.Name).ToListAsync();
}

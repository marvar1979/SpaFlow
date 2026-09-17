using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Employees;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    public List<Employee> Employees { get; set; } = [];
    public async Task OnGetAsync() => Employees = await db.Employees.AsNoTracking().Include(x => x.Branch).Where(x => x.SpaBusinessId == (tenant.SpaBusinessId ?? 0) && x.IsActive).OrderBy(x => x.FullName).ToListAsync();
}

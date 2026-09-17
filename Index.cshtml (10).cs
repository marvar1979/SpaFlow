using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Appointments;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    [BindProperty(SupportsGet = true)] public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public List<Appointment> Appointments { get; set; } = [];

    public async Task OnGetAsync()
    {
        var start = Date.ToDateTime(TimeOnly.MinValue);
        var end = start.AddDays(1);
        var spaId = tenant.SpaBusinessId ?? 0;
        var branchId = tenant.BranchId;
        Appointments = await db.Appointments.AsNoTracking()
            .Include(x => x.Client).Include(x => x.Employee).Include(x => x.Room)
            .Include(x => x.Items).ThenInclude(x => x.SpaService)
            .Where(x => x.SpaBusinessId == spaId && (!branchId.HasValue || x.BranchId == branchId) && x.StartAt >= start && x.StartAt < end)
            .OrderBy(x => x.StartAt).ToListAsync();
    }
}

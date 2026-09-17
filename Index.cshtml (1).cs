using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Dashboard;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    public int TodayAppointments { get; set; }
    public int ConfirmedAppointments { get; set; }
    public int ActiveClients { get; set; }
    public decimal TodayRevenue { get; set; }
    public List<Appointment> Upcoming { get; set; } = [];
    public List<Product> LowStock { get; set; } = [];

    public async Task OnGetAsync()
    {
        var spaId = tenant.SpaBusinessId ?? 0;
        var branchId = tenant.BranchId;
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var appointments = db.Appointments.AsNoTracking()
            .Where(x => x.SpaBusinessId == spaId && (!branchId.HasValue || x.BranchId == branchId));

        TodayAppointments = await appointments.CountAsync(x => x.StartAt >= today && x.StartAt < tomorrow && x.Status != AppointmentStatus.Cancelled);
        ConfirmedAppointments = await appointments.CountAsync(x => x.StartAt >= today && x.StartAt < tomorrow && x.Status == AppointmentStatus.Confirmed);
        ActiveClients = await db.Clients.CountAsync(x => x.SpaBusinessId == spaId && x.IsActive);
        TodayRevenue = await db.Payments.Where(x => x.SpaBusinessId == spaId && x.Status == PaymentStatus.Paid && x.PaidAtUtc >= today.ToUniversalTime() && x.PaidAtUtc < tomorrow.ToUniversalTime()).SumAsync(x => (decimal?)x.Amount) ?? 0;

        Upcoming = await appointments.Include(x => x.Client).Include(x => x.Employee)
            .Where(x => x.StartAt >= DateTime.Now && x.Status != AppointmentStatus.Cancelled)
            .OrderBy(x => x.StartAt).Take(8).ToListAsync();

        LowStock = await db.Products.AsNoTracking()
            .Where(x => x.SpaBusinessId == spaId && x.IsActive && x.Stock <= x.MinimumStock)
            .OrderBy(x => x.Stock).Take(8).ToListAsync();
    }
}

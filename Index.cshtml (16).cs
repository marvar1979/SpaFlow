using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Reports;

public class IndexModel(ApplicationDbContext db, ITenantContext tenant) : PageModel
{
    [BindProperty(SupportsGet=true)] public DateOnly From { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
    [BindProperty(SupportsGet=true)] public DateOnly To { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public decimal Revenue { get; set; }
    public int CompletedAppointments { get; set; }
    public decimal AverageTicket { get; set; }
    public int NewClients { get; set; }
    public List<TopServiceRow> TopServices { get; set; } = [];
    public record TopServiceRow(string Name, int Quantity, decimal Revenue);

    public async Task OnGetAsync()
    {
        var spaId = tenant.SpaBusinessId ?? 0;
        var start = From.ToDateTime(TimeOnly.MinValue);
        var end = To.AddDays(1).ToDateTime(TimeOnly.MinValue);
        Revenue = await db.Payments.Where(x => x.SpaBusinessId == spaId && x.Status == PaymentStatus.Paid && x.PaidAtUtc >= start.ToUniversalTime() && x.PaidAtUtc < end.ToUniversalTime()).SumAsync(x => (decimal?)x.Amount) ?? 0;
        CompletedAppointments = await db.Appointments.CountAsync(x => x.SpaBusinessId == spaId && x.Status == AppointmentStatus.Completed && x.StartAt >= start && x.StartAt < end);
        AverageTicket = CompletedAppointments == 0 ? 0 : Revenue / CompletedAppointments;
        NewClients = await db.Clients.CountAsync(x => x.SpaBusinessId == spaId && x.CreatedAtUtc >= start.ToUniversalTime() && x.CreatedAtUtc < end.ToUniversalTime());
        TopServices = await db.AppointmentItems.Where(x => x.Appointment.SpaBusinessId == spaId && x.Appointment.StartAt >= start && x.Appointment.StartAt < end && x.Appointment.Status != AppointmentStatus.Cancelled)
            .GroupBy(x => x.SpaService.Name).Select(g => new TopServiceRow(g.Key, g.Sum(x => x.Quantity), g.Sum(x => x.UnitPrice * x.Quantity))).OrderByDescending(x => x.Quantity).Take(10).ToListAsync();
    }
}

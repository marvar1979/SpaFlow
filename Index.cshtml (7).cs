using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Booking;

public class IndexModel(ApplicationDbContext db, IAppointmentService appointmentService) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();
    public SelectList ServiceOptions { get; set; } = null!;
    public SelectList EmployeeOptions { get; set; } = null!;

    public class InputModel
    {
        [Required, MaxLength(150)] public string FullName { get; set; } = string.Empty;
        [Required, MaxLength(30)] public string Phone { get; set; } = string.Empty;
        [EmailAddress, MaxLength(150)] public string? Email { get; set; }
        [Required] public int ServiceId { get; set; }
        [Required] public int EmployeeId { get; set; }
        [Required] public DateTime StartAt { get; set; } = DateTime.Now.AddDays(1);
        [MaxLength(500)] public string? Notes { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Debes autorizar el contacto para confirmar la reserva.")]
        public bool ContactConsent { get; set; }
    }

    public async Task OnGetAsync() => await LoadOptionsAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadOptionsAsync();
        if (!ModelState.IsValid) return Page();
        if (Input.StartAt < DateTime.Now.AddMinutes(30))
        {
            ModelState.AddModelError(string.Empty, "Selecciona un horario futuro.");
            return Page();
        }

        var business = await db.SpaBusinesses.AsNoTracking().FirstAsync(x => x.IsActive);
        var employee = await db.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Input.EmployeeId && x.SpaBusinessId == business.Id && x.IsActive);
        var service = await db.SpaServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Input.ServiceId && x.SpaBusinessId == business.Id && x.IsActive);
        if (employee is null || service is null)
        {
            ModelState.AddModelError(string.Empty, "Servicio o profesional inválido.");
            return Page();
        }

        var client = await db.Clients.FirstOrDefaultAsync(x => x.SpaBusinessId == business.Id && x.Phone == Input.Phone);
        if (client is null)
        {
            client = new Client { SpaBusinessId = business.Id, FullName = Input.FullName, Phone = Input.Phone, Email = Input.Email, MarketingConsent = false, TreatmentConsent = false };
            db.Clients.Add(client);
            await db.SaveChangesAsync();
        }
        else
        {
            client.FullName = Input.FullName;
            if (!string.IsNullOrWhiteSpace(Input.Email)) client.Email = Input.Email;
        }

        var appointment = new Appointment
        {
            SpaBusinessId = business.Id,
            BranchId = employee.BranchId,
            ClientId = client.Id,
            EmployeeId = employee.Id,
            StartAt = Input.StartAt,
            EndAt = Input.StartAt.AddMinutes(service.DurationMinutes),
            Status = AppointmentStatus.Pending,
            Source = AppointmentSource.Web,
            PaymentStatus = PaymentStatus.Pending,
            Notes = Input.Notes,
            Items = [new AppointmentItem { SpaServiceId = service.Id, Quantity = 1, DurationMinutes = service.DurationMinutes, UnitPrice = service.Price }]
        };
        await appointmentService.RecalculateAsync(appointment);
        var result = await appointmentService.ValidateAvailabilityAsync(appointment);
        if (!result.Ok)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return Page();
        }

        db.Appointments.Add(appointment);
        await db.SaveChangesAsync();
        return RedirectToPage("Success", new { id = appointment.Id });
    }

    private async Task LoadOptionsAsync()
    {
        var businessId = await db.SpaBusinesses.Where(x => x.IsActive).Select(x => x.Id).FirstOrDefaultAsync();
        ServiceOptions = new SelectList(await db.SpaServices.Where(x => x.SpaBusinessId == businessId && x.IsActive).OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
        EmployeeOptions = new SelectList(await db.Employees.Where(x => x.SpaBusinessId == businessId && x.IsActive).OrderBy(x => x.FullName).ToListAsync(), "Id", "FullName");
    }
}

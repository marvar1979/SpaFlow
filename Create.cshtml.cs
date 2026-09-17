using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Services;

namespace SpaFlow.Web.Pages.Appointments;

public class CreateModel(ApplicationDbContext db, ITenantContext tenant, IAppointmentService appointmentService) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();
    public SelectList ClientOptions { get; set; } = null!;
    public SelectList ServiceOptions { get; set; } = null!;
    public SelectList EmployeeOptions { get; set; } = null!;
    public SelectList RoomOptions { get; set; } = null!;

    public class InputModel
    {
        [Required] public int ClientId { get; set; }
        [Required] public int ServiceId { get; set; }
        [Required] public int EmployeeId { get; set; }
        public int? RoomId { get; set; }
        [Required] public DateTime StartAt { get; set; } = DateTime.Now.AddHours(1);
        public AppointmentSource Source { get; set; } = AppointmentSource.Reception;
        [MaxLength(700)] public string? Notes { get; set; }
    }

    public async Task OnGetAsync() => await LoadOptionsAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadOptionsAsync();
        if (!ModelState.IsValid) return Page();

        var spaId = tenant.SpaBusinessId ?? 0;
        var branchId = tenant.BranchId ?? await db.Branches.Where(x => x.SpaBusinessId == spaId).Select(x => x.Id).FirstAsync();
        var service = await db.SpaServices.FirstOrDefaultAsync(x => x.Id == Input.ServiceId && x.SpaBusinessId == spaId);
        if (service is null) { ModelState.AddModelError(string.Empty, "Servicio inválido."); return Page(); }

        var appointment = new Appointment
        {
            SpaBusinessId = spaId,
            BranchId = branchId,
            ClientId = Input.ClientId,
            EmployeeId = Input.EmployeeId,
            RoomId = Input.RoomId,
            StartAt = Input.StartAt,
            EndAt = Input.StartAt.AddMinutes(service.DurationMinutes),
            Source = Input.Source,
            Notes = Input.Notes,
            Status = AppointmentStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            CreatedByUserId = tenant.UserId,
            Items = [new AppointmentItem { SpaServiceId = service.Id, Quantity = 1, DurationMinutes = service.DurationMinutes, UnitPrice = service.Price }]
        };
        await appointmentService.RecalculateAsync(appointment);
        var availability = await appointmentService.ValidateAvailabilityAsync(appointment);
        if (!availability.Ok) { ModelState.AddModelError(string.Empty, availability.Error!); return Page(); }

        db.Appointments.Add(appointment);
        await db.SaveChangesAsync();
        TempData["Success"] = "Reserva creada correctamente.";
        return RedirectToPage("Index", new { Date = DateOnly.FromDateTime(Input.StartAt) });
    }

    private async Task LoadOptionsAsync()
    {
        var spaId = tenant.SpaBusinessId ?? 0;
        var branchId = tenant.BranchId;
        ClientOptions = new SelectList(await db.Clients.Where(x => x.SpaBusinessId == spaId && x.IsActive).OrderBy(x => x.FullName).ToListAsync(), "Id", "FullName");
        ServiceOptions = new SelectList(await db.SpaServices.Where(x => x.SpaBusinessId == spaId && x.IsActive).OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
        EmployeeOptions = new SelectList(await db.Employees.Where(x => x.SpaBusinessId == spaId && x.IsActive && (!branchId.HasValue || x.BranchId == branchId)).OrderBy(x => x.FullName).ToListAsync(), "Id", "FullName");
        RoomOptions = new SelectList(await db.Rooms.Where(x => x.SpaBusinessId == spaId && x.IsActive && (!branchId.HasValue || x.BranchId == branchId)).OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
    }
}

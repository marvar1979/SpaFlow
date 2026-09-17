using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;

namespace SpaFlow.Web.Services;

public class AppointmentService(ApplicationDbContext db) : IAppointmentService
{
    public async Task<(bool Ok, string? Error)> ValidateAvailabilityAsync(Appointment appointment, int? ignoreAppointmentId = null)
    {
        if (appointment.EndAt <= appointment.StartAt)
            return (false, "La hora de fin debe ser posterior a la hora de inicio.");

        var baseQuery = db.Appointments.AsNoTracking().Where(x =>
            x.BranchId == appointment.BranchId &&
            x.Status != AppointmentStatus.Cancelled &&
            x.Status != AppointmentStatus.NoShow &&
            x.StartAt < appointment.EndAt && x.EndAt > appointment.StartAt);

        if (ignoreAppointmentId.HasValue)
            baseQuery = baseQuery.Where(x => x.Id != ignoreAppointmentId.Value);

        if (await baseQuery.AnyAsync(x => x.EmployeeId == appointment.EmployeeId))
            return (false, "El profesional ya tiene una reserva que se cruza con ese horario.");

        if (appointment.RoomId.HasValue && await baseQuery.AnyAsync(x => x.RoomId == appointment.RoomId))
            return (false, "La cabina seleccionada ya está ocupada en ese horario.");

        return (true, null);
    }

    public Task RecalculateAsync(Appointment appointment)
    {
        appointment.Total = appointment.Items.Sum(x => x.UnitPrice * x.Quantity) - appointment.Discount;
        return Task.CompletedTask;
    }
}

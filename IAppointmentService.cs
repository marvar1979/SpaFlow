using SpaFlow.Web.Models;

namespace SpaFlow.Web.Services;

public interface IAppointmentService
{
    Task<(bool Ok, string? Error)> ValidateAvailabilityAsync(Appointment appointment, int? ignoreAppointmentId = null);
    Task RecalculateAsync(Appointment appointment);
}

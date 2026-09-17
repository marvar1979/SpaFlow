using System.ComponentModel.DataAnnotations;

namespace SpaFlow.Web.Models;

public class Client : BaseEntity
{
    public int SpaBusinessId { get; set; }
    [Required, MaxLength(150)] public string FullName { get; set; } = string.Empty;
    [MaxLength(30)] public string? Phone { get; set; }
    [MaxLength(150), EmailAddress] public string? Email { get; set; }
    public DateOnly? BirthDate { get; set; }
    [MaxLength(500)] public string? Notes { get; set; }
    [MaxLength(500)] public string? Allergies { get; set; }
    [MaxLength(500)] public string? Contraindications { get; set; }
    public bool MarketingConsent { get; set; }
    public bool TreatmentConsent { get; set; }
    public int LoyaltyPoints { get; set; }
    public DateTime? LastVisitAtUtc { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

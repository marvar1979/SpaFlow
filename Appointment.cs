using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaFlow.Web.Models;

public class Appointment : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int BranchId { get; set; }
    public int ClientId { get; set; }
    public int EmployeeId { get; set; }
    public int? RoomId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public AppointmentSource Source { get; set; } = AppointmentSource.Reception;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    [Column(TypeName = "decimal(18,2)")] public decimal Total { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Discount { get; set; }
    [MaxLength(700)] public string? Notes { get; set; }
    [MaxLength(450)] public string? CreatedByUserId { get; set; }
    public Client Client { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public Room? Room { get; set; }
    public Branch Branch { get; set; } = null!;
    public ICollection<AppointmentItem> Items { get; set; } = new List<AppointmentItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class AppointmentItem : BaseEntity
{
    public int AppointmentId { get; set; }
    public int SpaServiceId { get; set; }
    public int Quantity { get; set; } = 1;
    public int DurationMinutes { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal UnitPrice { get; set; }
    public Appointment Appointment { get; set; } = null!;
    public SpaService SpaService { get; set; } = null!;
}

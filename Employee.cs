using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaFlow.Web.Models;

public class Employee : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int BranchId { get; set; }
    public string? ApplicationUserId { get; set; }
    [Required, MaxLength(150)] public string FullName { get; set; } = string.Empty;
    [MaxLength(120)] public string? Specialty { get; set; }
    [MaxLength(30)] public string? Phone { get; set; }
    [MaxLength(150), EmailAddress] public string? Email { get; set; }
    [Column(TypeName = "decimal(5,2)")] public decimal CommissionPercent { get; set; }
    public Branch Branch { get; set; } = null!;
    public ApplicationUser? ApplicationUser { get; set; }
    public ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();
}

public class EmployeeService
{
    public int EmployeeId { get; set; }
    public int SpaServiceId { get; set; }
    public Employee Employee { get; set; } = null!;
    public SpaService SpaService { get; set; } = null!;
}

public class EmployeeSchedule : BaseEntity
{
    public int EmployeeId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsDayOff { get; set; }
    public Employee Employee { get; set; } = null!;
}

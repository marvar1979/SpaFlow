using System.ComponentModel.DataAnnotations;

namespace SpaFlow.Web.Models;

public class AuditLog : BaseEntity
{
    public int? SpaBusinessId { get; set; }
    [MaxLength(450)] public string? UserId { get; set; }
    [Required, MaxLength(80)] public string Action { get; set; } = string.Empty;
    [Required, MaxLength(120)] public string EntityName { get; set; } = string.Empty;
    [MaxLength(100)] public string? EntityId { get; set; }
    [MaxLength(2000)] public string? Details { get; set; }
    [MaxLength(64)] public string? IpAddress { get; set; }
}

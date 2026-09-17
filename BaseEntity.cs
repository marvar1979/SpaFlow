using System.ComponentModel.DataAnnotations;

namespace SpaFlow.Web.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
}

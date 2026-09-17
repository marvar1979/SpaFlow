using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaFlow.Web.Models;

public class ServiceCategory : BaseEntity
{
    public int SpaBusinessId { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public ICollection<SpaService> Services { get; set; } = new List<SpaService>();
}

public class SpaService : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int ServiceCategoryId { get; set; }
    [Required, MaxLength(140)] public string Name { get; set; } = string.Empty;
    [MaxLength(600)] public string? Description { get; set; }
    public int DurationMinutes { get; set; } = 60;
    [Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }
    public bool RequiresRoom { get; set; } = true;
    [Column(TypeName = "decimal(5,2)")] public decimal DefaultCommissionPercent { get; set; }
    public ServiceCategory Category { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace SpaFlow.Web.Models;

public class SpaBusiness : BaseEntity
{
    [Required, MaxLength(150)] public string Name { get; set; } = string.Empty;
    [MaxLength(200)] public string? LegalName { get; set; }
    [MaxLength(30)] public string? Phone { get; set; }
    [MaxLength(150), EmailAddress] public string? Email { get; set; }
    [MaxLength(250)] public string? Address { get; set; }
    [MaxLength(500)] public string? LogoUrl { get; set; }
    [MaxLength(10)] public string Currency { get; set; } = "BOB";
    [MaxLength(80)] public string TimeZoneId { get; set; } = "America/La_Paz";
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
}

public class Branch : BaseEntity
{
    public int SpaBusinessId { get; set; }
    [Required, MaxLength(120)] public string Name { get; set; } = string.Empty;
    [MaxLength(250)] public string? Address { get; set; }
    [MaxLength(30)] public string? Phone { get; set; }
    public SpaBusiness SpaBusiness { get; set; } = null!;
}

public class Room : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int BranchId { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
    [MaxLength(250)] public string? Description { get; set; }
    public int Capacity { get; set; } = 1;
    public Branch Branch { get; set; } = null!;
}

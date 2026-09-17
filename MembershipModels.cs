using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaFlow.Web.Models;

public class MembershipPlan : BaseEntity
{
    public int SpaBusinessId { get; set; }
    [Required, MaxLength(120)] public string Name { get; set; } = string.Empty;
    [MaxLength(500)] public string? Description { get; set; }
    public int DurationDays { get; set; } = 30;
    [Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }
    [Column(TypeName = "decimal(5,2)")] public decimal ServiceDiscountPercent { get; set; }
}

public class ClientMembership : BaseEntity
{
    public int SpaBusinessId { get; set; }
    public int ClientId { get; set; }
    public int MembershipPlanId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Client Client { get; set; } = null!;
    public MembershipPlan MembershipPlan { get; set; } = null!;
}

public class GiftCard : BaseEntity
{
    public int SpaBusinessId { get; set; }
    [Required, MaxLength(50)] public string Code { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")] public decimal InitialBalance { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal CurrentBalance { get; set; }
    public DateOnly? ExpiresOn { get; set; }
    public int? AssignedClientId { get; set; }
    public Client? AssignedClient { get; set; }
}

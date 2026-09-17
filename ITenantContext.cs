namespace SpaFlow.Web.Services;

public interface ITenantContext
{
    int? SpaBusinessId { get; }
    int? BranchId { get; }
    string? UserId { get; }
}

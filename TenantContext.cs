using Microsoft.AspNetCore.Identity;
using SpaFlow.Web.Models;

namespace SpaFlow.Web.Services;

public class TenantContext(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) : ITenantContext
{
    private ApplicationUser? CurrentUser
    {
        get
        {
            var principal = httpContextAccessor.HttpContext?.User;
            if (principal?.Identity?.IsAuthenticated != true) return null;
            return userManager.GetUserAsync(principal).GetAwaiter().GetResult();
        }
    }

    public int? SpaBusinessId => CurrentUser?.SpaBusinessId;
    public int? BranchId => CurrentUser?.BranchId;
    public string? UserId => CurrentUser?.Id;
}

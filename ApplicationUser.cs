using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SpaFlow.Web.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;
    public int? SpaBusinessId { get; set; }
    public int? BranchId { get; set; }
    public SpaBusiness? SpaBusiness { get; set; }
    public Branch? Branch { get; set; }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Models;

namespace SpaFlow.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<SpaBusiness> SpaBusinesses => Set<SpaBusiness>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<SpaService> SpaServices => Set<SpaService>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeeService> EmployeeServices => Set<EmployeeService>();
    public DbSet<EmployeeSchedule> EmployeeSchedules => Set<EmployeeSchedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AppointmentItem> AppointmentItems => Set<AppointmentItem>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<ClientMembership> ClientMemberships => Set<ClientMembership>();
    public DbSet<GiftCard> GiftCards => Set<GiftCard>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<EmployeeService>().HasKey(x => new { x.EmployeeId, x.SpaServiceId });
        builder.Entity<EmployeeService>()
            .HasOne(x => x.Employee).WithMany(x => x.EmployeeServices)
            .HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<EmployeeService>()
            .HasOne(x => x.SpaService).WithMany()
            .HasForeignKey(x => x.SpaServiceId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>().HasIndex(x => new { x.BranchId, x.StartAt });
        builder.Entity<Appointment>().HasIndex(x => new { x.EmployeeId, x.StartAt, x.EndAt });
        builder.Entity<Client>().HasIndex(x => new { x.SpaBusinessId, x.Phone });
        builder.Entity<Product>().HasIndex(x => new { x.SpaBusinessId, x.Sku });
        builder.Entity<GiftCard>().HasIndex(x => new { x.SpaBusinessId, x.Code }).IsUnique();

        foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            if (property.GetPrecision() is null)
                property.SetPrecision(18);
            if (property.GetScale() is null)
                property.SetScale(2);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAtUtc = DateTime.UtcNow;
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}

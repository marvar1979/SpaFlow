using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Models;
using SpaFlow.Web.Security;

namespace SpaFlow.Web.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        var migrations = await db.Database.GetMigrationsAsync();
        if (migrations.Any())
            await db.Database.MigrateAsync();
        else
            await db.Database.EnsureCreatedAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in Roles.All)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        var business = await db.SpaBusinesses.FirstOrDefaultAsync();
        if (business is null)
        {
            business = new SpaBusiness
            {
                Name = "SpaFlow Demo",
                LegalName = "SpaFlow Demo",
                Phone = "+591 70000000",
                Email = "contacto@spaflow.local",
                Currency = "BOB"
            };
            db.SpaBusinesses.Add(business);
            await db.SaveChangesAsync();

            db.Branches.Add(new Branch
            {
                SpaBusinessId = business.Id,
                Name = "Sucursal Principal",
                Address = "Configurar dirección"
            });
            await db.SaveChangesAsync();
        }

        var branch = await db.Branches.FirstAsync(x => x.SpaBusinessId == business.Id);

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var email = configuration["AdminSeed:Email"] ?? "admin@spaflow.local";
        var password = configuration["AdminSeed:Password"] ?? "ChangeMe123!";
        var fullName = configuration["AdminSeed:FullName"] ?? "Administrador SpaFlow";

        var admin = await userManager.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName,
                SpaBusinessId = business.Id,
                BranchId = branch.Id
            };
            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
            await userManager.AddToRolesAsync(admin, [Roles.Admin, Roles.Manager]);
        }

        if (!await db.ServiceCategories.AnyAsync(x => x.SpaBusinessId == business.Id))
        {
            var categories = new[]
            {
                new ServiceCategory { SpaBusinessId = business.Id, Name = "Masajes", DisplayOrder = 1 },
                new ServiceCategory { SpaBusinessId = business.Id, Name = "Faciales", DisplayOrder = 2 },
                new ServiceCategory { SpaBusinessId = business.Id, Name = "Corporales", DisplayOrder = 3 },
                new ServiceCategory { SpaBusinessId = business.Id, Name = "Manicure y Pedicure", DisplayOrder = 4 }
            };
            db.ServiceCategories.AddRange(categories);
            await db.SaveChangesAsync();

            var massage = categories[0];
            var facial = categories[1];
            db.SpaServices.AddRange(
                new SpaService { SpaBusinessId = business.Id, ServiceCategoryId = massage.Id, Name = "Masaje relajante", DurationMinutes = 60, Price = 180, DefaultCommissionPercent = 10 },
                new SpaService { SpaBusinessId = business.Id, ServiceCategoryId = massage.Id, Name = "Masaje descontracturante", DurationMinutes = 60, Price = 220, DefaultCommissionPercent = 10 },
                new SpaService { SpaBusinessId = business.Id, ServiceCategoryId = facial.Id, Name = "Limpieza facial", DurationMinutes = 75, Price = 250, DefaultCommissionPercent = 10 }
            );
            db.Rooms.AddRange(
                new Room { SpaBusinessId = business.Id, BranchId = branch.Id, Name = "Cabina 1" },
                new Room { SpaBusinessId = business.Id, BranchId = branch.Id, Name = "Cabina 2" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.Employees.AnyAsync(x => x.SpaBusinessId == business.Id))
        {
            var employee = new Employee
            {
                SpaBusinessId = business.Id,
                BranchId = branch.Id,
                FullName = "Profesional Demo",
                Specialty = "Masajes y bienestar",
                Phone = "+591 70000001",
                CommissionPercent = 10
            };
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            var serviceIds = await db.SpaServices.Where(x => x.SpaBusinessId == business.Id).Select(x => x.Id).ToListAsync();
            db.EmployeeServices.AddRange(serviceIds.Select(id => new EmployeeService { EmployeeId = employee.Id, SpaServiceId = id }));
            await db.SaveChangesAsync();
        }

        if (!await db.Clients.AnyAsync(x => x.SpaBusinessId == business.Id))
        {
            db.Clients.Add(new Client { SpaBusinessId = business.Id, FullName = "Cliente Demo", Phone = "+591 70000002", TreatmentConsent = true });
            await db.SaveChangesAsync();
        }

        if (!await db.ProductCategories.AnyAsync(x => x.SpaBusinessId == business.Id))
        {
            var category = new ProductCategory { SpaBusinessId = business.Id, Name = "Cuidado personal" };
            db.ProductCategories.Add(category);
            await db.SaveChangesAsync();
            db.Products.Add(new Product { SpaBusinessId = business.Id, ProductCategoryId = category.Id, Name = "Aceite corporal", Sku = "SPA-001", Cost = 35, SalePrice = 65, Stock = 10, MinimumStock = 3 });
            await db.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpaFlow.Web.Data;
using SpaFlow.Web.Models;
using SpaFlow.Web.Security;
using SpaFlow.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Management", policy => policy.RequireRole(Roles.Admin, Roles.Manager));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Dashboard");
    options.Conventions.AuthorizeFolder("/Appointments");
    options.Conventions.AuthorizeFolder("/Clients");
    options.Conventions.AuthorizeFolder("/Services");
    options.Conventions.AuthorizeFolder("/Employees");
    options.Conventions.AuthorizeFolder("/Inventory");
    options.Conventions.AuthorizeFolder("/Reports");
    options.Conventions.AuthorizeFolder("/Settings", "Management");
    options.Conventions.AllowAnonymousToFolder("/Booking");
});

builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

app.Run();

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Business.Concrete;
using RentACar.Business.Validators.Car;
using RentACar.DataAccess;
using RentACar.WebUI.Middleware;
using RentACar.WebUI.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Database
builder.Services.AddDbContext<RentACarDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41))
    )
);
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    
    options.ViewLocationFormats.Add("/Views/Admin/{1}/{0}.cshtml");

    
    options.ViewLocationFormats.Add("/Views/Site/{1}/{0}.cshtml");
});
// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<RentACarDbContext>()
.AddDefaultTokenProviders();

// Email
builder.Services.AddTransient<IEmailSender, EmailSender>();

// HttpContext
builder.Services.AddHttpContextAccessor();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CarCreateValidator>();

// Business Servisleri
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserService, RentACar.Business.Concrete.UserService>();
builder.Services.AddScoped<ISiteContactService, SiteContactService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IBrandService, BrandService>();

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("PersonelOnly", policy => policy.RequireRole("Personel"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});

var app = builder.Build();

// Middleware Pipeline
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Site}/{action=Index}/{id?}"
);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Seed sırasında hata oluştu.");
    }
}

app.Run();
public partial class Program { }
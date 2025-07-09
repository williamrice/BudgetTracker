using BudgetTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BudgetTracker.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using BudgetTracker.Services;
using BudgetTracker.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
                       builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BudgetTrackerContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddScoped<BudgetTracker.Web.Services.DemoService>();
builder.Services.AddHostedService<BudgetTracker.Web.Services.DemoResetBackgroundService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

//Identity Services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddEntityFrameworkStores<BudgetTrackerContext>()
  .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Welcome";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Home/Welcome";
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapRazorPages();

app.MapControllerRoute(
    name: "demo",
    pattern: "demo",
    defaults: new { controller = "Home", action = "DemoInfo" });

app.MapControllerRoute(
    name: "demo-start",
    pattern: "demo/start",
    defaults: new { controller = "Home", action = "Demo" });

app.MapControllerRoute(
    name: "demo-reset",
    pattern: "demo/reset",
    defaults: new { controller = "Home", action = "ResetDemo" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var seeder = services.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


app.Run();

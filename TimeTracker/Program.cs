using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;
using TimeTracker.Services;
using TimeTracker.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

//
// Add services to the container.
//
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
		options.SignIn.RequireConfirmedAccount = false
	)
	.AddRoles<ApplicationRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//
// Require authenticated user.
//
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireAdministratorRole",
		policy => policy.RequireRole(Roles.Admin));
	
	options.FallbackPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
});

builder.Services.Configure<IdentityOptions>(options =>
{
    //
    // Password settings.
    //!! Important requirements have been removed for testing,
    //!! which are worth having included in the Release version.
    //!! It may be worth disabling the requirements for the debug version only.
    //
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

    //
    // Lockout settings.
    //
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //
    // User settings.
    //
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    //
    // Cookie settings
    //
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

//
// Services.
//
builder.Services.AddScoped<IClosedWeekService, ClosedWeekService>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();

var app = builder.Build();

//
// Seed the DB.
//
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	try
	{
		DbContextSeed.InitializeAsync(services).Wait();
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "Error occurred seeding the DB.");
	}
}

//
// Configure the HTTP request pipeline.
//
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    //
    // The default HSTS value is 30 days. You may want to change this for production scenarios,
    // see https://aka.ms/aspnetcore-hsts.
    //
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

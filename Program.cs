using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WDP2024Assignment2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();
////////////////////////////////
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

// Update database
builder.Services.BuildServiceProvider()
    .GetRequiredService<ApplicationDbContext>()
    .Database.Migrate();

///// Add admin role
string role = "admin";
string email = "admin@asp.net";
string password = "Wdp-2024";
var roleManager = builder.Services.BuildServiceProvider()
    .GetRequiredService<RoleManager<IdentityRole>>();
bool roleExists = await roleManager.RoleExistsAsync(role);
IdentityRole userRole = new IdentityRole(role);
if (!roleExists)
{
    await roleManager.CreateAsync(userRole);
}
// Add admin user
var userAdmin = builder.Services.BuildServiceProvider()
    .GetRequiredService<UserManager<IdentityUser>>();
IdentityUser identityUser = await userAdmin.FindByEmailAsync(email);
if (identityUser == null)
{
    identityUser = new IdentityUser
    {
        UserName = email,
        Email = email,
        EmailConfirmed = true
    };
    var createUser = await userAdmin.CreateAsync(identityUser, password);
}
// Assign admin role to user
if (identityUser != null)
{
    await userAdmin.AddToRoleAsync(identityUser, role);
}

// Add member without role
email = "member@asp.net";
password = "Wdp-2024";
// Add member user
var member = builder.Services.BuildServiceProvider()
    .GetRequiredService<UserManager<IdentityUser>>();
identityUser = await member.FindByEmailAsync(email);
if (identityUser == null)
{
    identityUser = new IdentityUser
    {
        UserName = email,
        Email = email,
        EmailConfirmed = true
    };
    var createUser = await member.CreateAsync(identityUser, password);
}
/////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

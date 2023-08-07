

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;
using Pronia.Identity;
using Pronia.Services.Implementation;
using Pronia.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});



builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.AllowedForNewUsers = false;
})
.AddEntityFrameworkStores<AppDbContext>();



builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureApplicationCookie(c =>
{
    c.LoginPath = "/Auth/Login";
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
         name: "areas",
         pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
       );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();


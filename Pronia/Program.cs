

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;
using Pronia.Services.Implementation;
using Pronia.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
app.UseStaticFiles();

app.MapControllerRoute(
         name: "areas",
         pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
       );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();


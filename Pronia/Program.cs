using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});

var app = builder.Build();
app.UseStaticFiles();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
	);

app.Run();

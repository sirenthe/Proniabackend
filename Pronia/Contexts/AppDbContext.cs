using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.Contexts
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Feature> Features { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Feature>().HasQueryFilter(f => !f.IsDeleted);
			base.OnModelCreating(modelBuilder);
		}
	}

}
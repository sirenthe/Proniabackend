using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Identity;
using Pronia.Models;
using Pronia.Models.Common;

namespace Pronia.Contexts
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		private readonly IHttpContextAccessor _contextAccessor;
		public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor) : base(options)
		{
			_contextAccessor = contextAccessor;
		}

		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Feature> Features { get; set; }
		public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Feature>().HasQueryFilter(f => !f.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            base.OnModelCreating(modelBuilder);
		}
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
			string? name = _contextAccessor.HttpContext.User.Identity.IsAuthenticated ?
				_contextAccessor.HttpContext.User.Identity.Name : "Admin";
		var entries=ChangeTracker.Entries<BaseSectionEntity>();
			foreach (var entry in entries)
			{
				switch (entry.State)
				{ 
						case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = name;
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = name;
                        break;
						case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
						entry.Entity.UpdatedBy = name;
                        break;
						default: break;

				}
				
			}
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }

}
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class AppDbContext : IdentityDbContext<AppUser,AppRole,int>
    {
		//public AppDbContext() { }
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

		#region Tables
		public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
		#endregion

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Favorite>()
			  .HasKey(x => new { x.AppUserId, x.PostId });

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			
		}
	}
}

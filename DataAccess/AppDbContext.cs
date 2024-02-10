using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace DataAccess
{
	public class AppDbContext : IdentityDbContext<AppUser,AppRole,int>
    {
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

		#region Tables
		public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
		public DbSet<Follow> Follows { get; set; }
        public DbSet<Message> Messages { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Favorite>()
			  .HasKey(x => new { x.AppUserId, x.PostId });
						//////////////////
			builder.Entity<Follow>()
				.HasKey(x => new { x.FollowingId, x.FollowedId });

			builder.Entity<Follow>()
				.HasOne(f => f.Following)
				.WithMany(u => u.Followings)
				.HasForeignKey(f => f.FollowingId);

			builder.Entity<Follow>()
				.HasOne(f => f.Followed)
				.WithMany(u => u.Followeds)
				.HasForeignKey(f => f.FollowedId);
						///////////////////
			builder.Entity<Message>()
				.HasOne(s => s.Sender)
				.WithMany(m => m.SendedMessages)
				.HasForeignKey(s => s.SenderId);

            builder.Entity<Message>()
                .HasOne(s => s.Receiver)
                .WithMany(m => m.ReceivedMessages)
                .HasForeignKey(s => s.ReceiverId);


        }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			
		}
	}
}

using Jobsity.Web.Application.Bots;
using Jobsity.Web.Application.Messages;
using Jobsity.Web.Application.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Web.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);

			modelBuilder
				.Entity<Message>()
				.ToTable("Messages")
				.HasKey(message => message.Id);

			modelBuilder
				.Entity<Message>()
				.HasOne(message => message.User);

			modelBuilder
                .Entity<Bot>()
                .ToTable("Bots")
                .HasKey(bot => bot.Id);

			modelBuilder
				.Entity<User>()
				.HasMany(user => user.Messages);
		}

		public DbSet<Message> Messages { get; set; }

		public DbSet<Bot> Bots { get; set; }
    }
}

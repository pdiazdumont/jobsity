using Microsoft.EntityFrameworkCore;

namespace Jobsity.EventProcessor.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder
				.Entity<Bot>()
				.ToTable("Bots")
				.HasKey(bot => bot.Id);
		}

		public DbSet<Bot> Bots { get; set; }
	}
}

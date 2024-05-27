namespace CryptoWorld.News.Data
{
	using Data.Models;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Article> Articles { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Comment> Comments { get; set; }

		public DbSet<Source> Sources { get; set; }

		public DbSet<Keyword> Keywords { get; set; }

		public DbSet<ArticleKeyword> ArticleKeywords { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<ArticleKeyword>()
				.HasKey(ak => new { ak.ArticleId, ak.KeywordId });

			base.OnModelCreating(builder);
		}
	}
}

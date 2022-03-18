namespace data.hybrid.repository.Context
{
    using data.hybrid.repository.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Test> Test { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
	}
}

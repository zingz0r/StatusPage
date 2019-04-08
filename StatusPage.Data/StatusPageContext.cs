using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StatusPage.Data.Entity;

namespace StatusPage.Data
{
    public class StatusPageContext : DbContext
    {
        public StatusPageContext(DbContextOptions<StatusPageContext> options)
               : base(options)
        {
            Database.Migrate();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlUseIdentityColumns();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<CheckResult> CheckResults { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestDetails> TestDetails { get; set; }
        public DbSet<Uptime> Uptimes { get; set; }
    }
}


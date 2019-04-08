using Microsoft.EntityFrameworkCore;
using StatusPage.Data.Entity;

namespace StatusPage.Data
{
    public class StatusPageContext : DbContext
    {
        public StatusPageContext(DbContextOptions<StatusPageContext> options)
               : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<CheckResult> CheckResults { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestDetails> TestDetails { get; set; }
        public DbSet<Uptime> Uptimes { get; set; }
    }
}


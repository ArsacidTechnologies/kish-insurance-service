using kish_insurance_service.Models;
using Microsoft.EntityFrameworkCore;

namespace kish_insurance_service
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InsuranceRequest> InsuranceRequests { get; set; }
        public DbSet<Coverage> Coverages { get; set; }
        public DbSet<CoverageType> CoverageTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coverage>()
                .Property(c => c.Capital)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<CoverageType>()
                .Property(c => c.MinCapital)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<CoverageType>()
                .Property(c => c.MaxCapital)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<CoverageType>()
                .Property(c => c.PremiumRate)
                .HasColumnType("decimal(5, 4)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
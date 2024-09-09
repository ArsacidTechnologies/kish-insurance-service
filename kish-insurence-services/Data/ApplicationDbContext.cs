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
    }
}
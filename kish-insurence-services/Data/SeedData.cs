using kish_insurance_service.Models;
using Microsoft.EntityFrameworkCore;

namespace kish_insurance_service.Data
{
    public static class SeedData
    {
        public static void SeedCoverageTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoverageType>().HasData(
                new CoverageType
                {
                    Id = 1,
                    Name = "Surgery",
                    PremiumRate = 0.0052m,
                    MinCapital = 5000m,
                    MaxCapital = 500000000m
                },
                new CoverageType
                {
                    Id = 2,
                    Name = "Dental",
                    PremiumRate = 0.0042m,
                    MinCapital = 4000m,
                    MaxCapital = 400000000m
                },
                new CoverageType
                {
                    Id = 3,
                    Name = "Hospitalization",
                    PremiumRate = 0.005m,
                    MinCapital = 2000m,
                    MaxCapital = 200000000m
                }
            );
        }
    }
}

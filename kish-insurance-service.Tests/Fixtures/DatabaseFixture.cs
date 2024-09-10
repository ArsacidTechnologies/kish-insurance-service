using kish_insurance_service;
using Microsoft.EntityFrameworkCore;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost;Database=TestInsuranceDB;User Id=sa;Password=Mehran@SQRootPass;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;")
            .Options;

        Context = new ApplicationDbContext(options);
        Context.Database.Migrate();  // Apply migrations to ensure the DB is created
    }

    public void Dispose()
    {
        //Context.Database.EnsureDeleted();  
        Context.Dispose();
    }
}

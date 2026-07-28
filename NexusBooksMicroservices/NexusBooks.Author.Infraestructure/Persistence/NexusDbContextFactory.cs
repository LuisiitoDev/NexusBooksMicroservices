using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NexusBooks.Author.Infraestructure.Persistence;

public sealed class NexusDbContextFactory : IDesignTimeDbContextFactory<NexusDbContext>
{
    public NexusDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NexusDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=NexusBooksAuthorDb;Trusted_Connection=True;TrustServerCertificate=True");

        return new NexusDbContext(optionsBuilder.Options);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NexusBooks.Book.Infraestructure.Persistence;

public sealed class NexusDbContextFactory : IDesignTimeDbContextFactory<NexusDbContext>
{
    public NexusDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NexusDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=NexusBooksBookDb;Username=postgres;Password=postgres");

        return new NexusDbContext(optionsBuilder.Options);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace NexusBooks.ShoppingCart.Infraestructure.Persistence;

public sealed class NexusDbContextFactory : IDesignTimeDbContextFactory<NexusDbContext>
{
    public NexusDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NexusDbContext>();
        optionsBuilder.UseMySql(
            "Server=localhost;Database=NexusBooksShoppingCartDb;User=root;Password=root",
            new MySqlServerVersion(new Version(8, 0, 36)));

        return new NexusDbContext(optionsBuilder.Options);
    }
}
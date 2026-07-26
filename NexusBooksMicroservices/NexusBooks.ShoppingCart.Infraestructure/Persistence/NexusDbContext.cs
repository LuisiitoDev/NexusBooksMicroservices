using Microsoft.EntityFrameworkCore;
using NexusBooks.ShoppingCart.Domain.Models;

namespace NexusBooks.ShoppingCart.Infraestructure.Persistence;

public sealed class NexusDbContext(DbContextOptions<NexusDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingCartSessionModel> ShoppingCarts => Set<ShoppingCartSessionModel>();
    public DbSet<ShoppingCartDetailSessionModel> ShoppingCartDetails => Set<ShoppingCartDetailSessionModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);
    }
}

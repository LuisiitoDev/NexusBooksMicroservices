using Microsoft.EntityFrameworkCore;
using NexusBooks.Author.Domain.Models;

namespace NexusBooks.Author.Infraestructure.Persistence;

public sealed class NexusDbContext(DbContextOptions<NexusDbContext> options) : DbContext(options)
{

    public DbSet<AuthorModel> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);
    }
}

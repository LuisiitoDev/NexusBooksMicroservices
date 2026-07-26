using Microsoft.EntityFrameworkCore;
using NexusBooks.Book.Domain.Models;

namespace NexusBooks.Book.Infraestructure.Persistence;

public sealed class NexusDbContext(DbContextOptions<NexusDbContext> options) : DbContext(options)
{
    public DbSet<BookModel> Books => Set<BookModel>();
    public DbSet<BookAuthorModel> BookAuthors => Set<BookAuthorModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusBooks.Author.Domain.Models;

namespace NexusBooks.Author.Infraestructure.Persistence;

internal sealed class AuthorConfiguration : IEntityTypeConfiguration<AuthorModel>
{
    public void Configure(EntityTypeBuilder<AuthorModel> builder)
    {
        builder.ToTable("Author");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.BirthDate).IsRequired();
        builder.Property(a => a.CreateAt).HasDefaultValueSql("GETDATE()").IsRequired();
    }
}

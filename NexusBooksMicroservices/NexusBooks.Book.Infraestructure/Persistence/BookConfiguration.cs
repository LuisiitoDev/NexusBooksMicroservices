using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusBooks.Book.Domain.Models;

namespace NexusBooks.Book.Infraestructure.Persistence;

internal sealed class BookConfiguration : IEntityTypeConfiguration<BookModel>
{
    public void Configure(EntityTypeBuilder<BookModel> builder)
    {
        builder.ToTable("Book");

        builder.HasKey(book => book.Id);

        builder.Property(book => book.Title).IsRequired();
        builder.Property(book => book.OverView).IsRequired();
        builder.Property(book => book.ISBN).IsRequired();
        builder.Property(book => book.PublicationDate).IsRequired();

        builder.HasMany(book => book.Authors)
            .WithOne(bookAuthor => bookAuthor.Book)
            .HasForeignKey(bookAuthor => bookAuthor.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

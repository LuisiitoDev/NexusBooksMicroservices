using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusBooks.Book.Domain.Models;

namespace NexusBooks.Book.Infraestructure.Persistence;

internal sealed class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthorModel>
{
    public void Configure(EntityTypeBuilder<BookAuthorModel> builder)
    {
        builder.ToTable("BookAuthor");

        builder.HasKey(bookAuthor => new { bookAuthor.BookId, bookAuthor.AuthorId });

        builder.Property(bookAuthor => bookAuthor.BookId).IsRequired();
        builder.Property(bookAuthor => bookAuthor.AuthorId).IsRequired();
    }
}

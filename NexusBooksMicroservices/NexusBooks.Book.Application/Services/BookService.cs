using Microsoft.EntityFrameworkCore;
using NexusBooks.Book.Application.Dtos;
using NexusBooks.Book.Application.Extensions;
using NexusBooks.Book.Application.Interfaces;
using NexusBooks.Book.Domain.Models;
using NexusBooks.Book.Infraestructure.Persistence;
using NexusBooks.Shared;

namespace NexusBooks.Book.Application.Services;

public sealed class BookService(NexusDbContext context) : IBookService
{
    public async Task<Result<DtoBook>> Create(DtoBook book, CancellationToken cancellation)
    {
        if (book is null)
            return new Error("Book is required", 400);

        var model = book.MapToModel();
        await context.Books.AddAsync(model, cancellation);

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(model.MapToDto());

        return new Error("Failed to create book");
    }

    public async Task<Result<List<DtoBook>>> GetAll(CancellationToken cancellation)
    {
        var books = await context.Books
            .AsNoTracking()
            .Include(book => book.Authors)
            .ToListAsync(cancellation);

        return new Success(books.Select(book => book.MapToDto()).ToList());
    }

    public async Task<Result<DtoBook>> GetById(long id, CancellationToken cancellation)
    {
        var book = await context.Books
            .AsNoTracking()
            .Include(book => book.Authors)
            .FirstOrDefaultAsync(book => book.Id == id, cancellation);

        if (book is null)
            return new Error("Book not found", 404);

        return new Success(book.MapToDto());
    }

    public async Task<Result<DtoBook>> Update(DtoBook book, CancellationToken cancellation)
    {
        if (book is null)
            return new Error("Book is required", 400);

        var existingBook = await context.Books
            .Include(book => book.Authors)
            .FirstOrDefaultAsync(bookItem => bookItem.Id == book.Id, cancellation);

        if (existingBook is null)
            return new Error("Book not found", 404);

        existingBook.Title = book.Title;
        existingBook.OverView = book.OverView;
        existingBook.ISBN = book.ISBN;
        existingBook.PublicationDate = book.PublicationDate;

        existingBook.Authors.Clear();
        foreach (var authorId in (book.AuthorIds ?? []).Distinct())
        {
            existingBook.Authors.Add(new BookAuthorModel
            {
                Book = existingBook,
                AuthorId = authorId
            });
        }

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(existingBook.MapToDto());

        return new Error("Failed to update book");
    }

    public async Task<Result> Delete(long id, CancellationToken cancellation)
    {
        var book = await context.Books
            .Include(book => book.Authors)
            .FirstOrDefaultAsync(bookItem => bookItem.Id == id, cancellation);

        if (book is null)
            return new Error("Book not found", 404);

        context.Books.Remove(book);

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(true);

        return new Error("Failed to delete book");
    }
}

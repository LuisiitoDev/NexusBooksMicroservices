using NexusBooks.Author.Application.Dtos;
using NexusBooks.Author.Application.Extensions;
using NexusBooks.Author.Application.Interfaces;
using NexusBooks.Author.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NexusBooks.Shared;

namespace NexusBooks.Author.Application.Services;

public sealed class AuthorService(NexusDbContext context) : IAuthorService
{
    public async Task<Result<DtoAuthor>> Create(DtoAuthor author, CancellationToken cancellation)
    {
        if (author is null)
            return new Error("Author is required", 400);

        var model = author.MapToModel();
        await context.Authors.AddAsync(model, cancellation);

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(model.MapToDto());


        return new Error("Failed to create author");
    }

    public async Task<Result<List<DtoAuthor>>> GetAll(CancellationToken cancellation)
    {
        var authors = await context.Authors
            .AsNoTracking()
            .ToListAsync(cancellation);

        return new Success(authors.Select(author => author.MapToDto()).ToList());
    }

    public async Task<Result<DtoAuthor>> GetById(long id, CancellationToken cancellation)
    {
        var author = await context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(author => author.Id == id, cancellation);

        if (author is null)
            return new Error("Author not found", 404);

        return new Success(author.MapToDto());
    }

    public async Task<Result<DtoAuthor>> Update(DtoAuthor author, CancellationToken cancellation)
    {
        if (author is null)
            return new Error("Author is required", 400);

        var existingAuthor = await context.Authors.FindAsync([author.Id], cancellation);

        if (existingAuthor is null)
            return new Error("Author not found", 404);

        existingAuthor.Name = author.Name;
        existingAuthor.BirthDate = author.BirthDate;

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(existingAuthor.MapToDto());

        return new Error("Failed to update author");
    }

    public async Task<Result> Delete(long id, CancellationToken cancellation)
    {
        var author = await context.Authors.FindAsync([id], cancellation);

        if (author is null)
            return new Error("Author not found", 404);

        context.Authors.Remove(author);

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(true);

        return new Error("Failed to delete author");
    }
}

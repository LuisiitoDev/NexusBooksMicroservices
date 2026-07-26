using NexusBooks.Author.Application.Dtos;
using NexusBooks.Shared;

namespace NexusBooks.Author.Application.Interfaces;

public interface IAuthorService
{
    Task<Result<DtoAuthor>> Create(DtoAuthor author, CancellationToken cancellation);
    Task<Result<List<DtoAuthor>>> GetAll(CancellationToken cancellation);
    Task<Result<DtoAuthor>> GetById(long id, CancellationToken cancellation);
    Task<Result<DtoAuthor>> Update(DtoAuthor author, CancellationToken cancellation);
    Task<Result> Delete(long id, CancellationToken cancellation);
}

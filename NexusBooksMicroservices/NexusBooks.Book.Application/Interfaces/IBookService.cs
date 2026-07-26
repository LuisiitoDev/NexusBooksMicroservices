using NexusBooks.Book.Application.Dtos;
using NexusBooks.Shared;

namespace NexusBooks.Book.Application.Interfaces;

public interface IBookService
{
    Task<Result<DtoBook>> Create(DtoBook book, CancellationToken cancellation);
    Task<Result<List<DtoBook>>> GetAll(CancellationToken cancellation);
    Task<Result<DtoBook>> GetById(long id, CancellationToken cancellation);
    Task<Result<DtoBook>> Update(DtoBook book, CancellationToken cancellation);
    Task<Result> Delete(long id, CancellationToken cancellation);
}

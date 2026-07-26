using NexusBooks.Shared;
using NexusBooks.ShoppingCart.Application.Dtos;

namespace NexusBooks.ShoppingCart.Application.Interfaces;

public interface IShoppingCartService
{
    Task<Result<DtoShoppingCartSession>> Create(DtoShoppingCartSession shoppingCart, CancellationToken cancellation);
    Task<Result<List<DtoShoppingCartSession>>> GetAll(CancellationToken cancellation);
    Task<Result<DtoShoppingCartSession>> GetById(Guid id, CancellationToken cancellation);
    Task<Result<DtoShoppingCartSession>> Update(DtoShoppingCartSession shoppingCart, CancellationToken cancellation);
    Task<Result> Delete(Guid id, CancellationToken cancellation);
}

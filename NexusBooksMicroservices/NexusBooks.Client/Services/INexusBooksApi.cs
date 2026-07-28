using NexusBooks.Client.Models;
using Refit;

namespace NexusBooks.Client.Services;

public interface INexusBooksApi
{
    [Get("/api/authors")]
    Task<List<AuthorDto>> GetAuthorsAsync(CancellationToken cancellationToken = default);

    [Get("/api/authors/{id}")]
    Task<AuthorDto?> GetAuthorByIdAsync(long id, CancellationToken cancellationToken = default);

    [Post("/api/authors")]
    Task<AuthorDto?> CreateAuthorAsync([Body] AuthorFormModel author, CancellationToken cancellationToken = default);

    [Put("/api/authors/{id}")]
    Task<AuthorDto?> UpdateAuthorAsync(long id, [Body] AuthorFormModel author, CancellationToken cancellationToken = default);

    [Delete("/api/authors/{id}")]
    Task DeleteAuthorAsync(long id, CancellationToken cancellationToken = default);

    [Get("/api/books")]
    Task<List<BookDto>> GetBooksAsync(CancellationToken cancellationToken = default);

    [Get("/api/books/{id}")]
    Task<BookDto?> GetBookByIdAsync(long id, CancellationToken cancellationToken = default);

    [Post("/api/books")]
    Task<BookDto?> CreateBookAsync([Body] BookFormModel book, CancellationToken cancellationToken = default);

    [Put("/api/books/{id}")]
    Task<BookDto?> UpdateBookAsync(long id, [Body] BookFormModel book, CancellationToken cancellationToken = default);

    [Delete("/api/books/{id}")]
    Task DeleteBookAsync(long id, CancellationToken cancellationToken = default);

    [Get("/api/shopping-carts")]
    Task<List<ShoppingCartSessionDto>> GetShoppingCartsAsync(CancellationToken cancellationToken = default);

    [Get("/api/shopping-carts/{id}")]
    Task<ShoppingCartSessionDto?> GetShoppingCartByIdAsync(Guid id, CancellationToken cancellationToken = default);

    [Post("/api/shopping-carts")]
    Task<ShoppingCartSessionDto?> CreateShoppingCartAsync([Body] ShoppingCartSessionDto shoppingCart, CancellationToken cancellationToken = default);

    [Put("/api/shopping-carts/{id}")]
    Task<ShoppingCartSessionDto?> UpdateShoppingCartAsync(Guid id, [Body] ShoppingCartSessionDto shoppingCart, CancellationToken cancellationToken = default);

    [Delete("/api/shopping-carts/{id}")]
    Task DeleteShoppingCartAsync(Guid id, CancellationToken cancellationToken = default);
}

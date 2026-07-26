using Microsoft.EntityFrameworkCore;
using NexusBooks.Shared;
using NexusBooks.ShoppingCart.Application.Dtos;
using NexusBooks.ShoppingCart.Application.Extensions;
using NexusBooks.ShoppingCart.Application.Interfaces;
using NexusBooks.ShoppingCart.Domain.Models;
using NexusBooks.ShoppingCart.Infraestructure.Persistence;

namespace NexusBooks.ShoppingCart.Application.Services;

public sealed class ShoppingCartService(NexusDbContext context) : IShoppingCartService
{
    public async Task<Result<DtoShoppingCartSession>> Create(DtoShoppingCartSession shoppingCart, CancellationToken cancellation)
    {
        if (shoppingCart is null)
            return new Error("Shopping cart is required", 400);

        var model = shoppingCart.MapToModel();
        await context.ShoppingCarts.AddAsync(model, cancellation);

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(model.MapToDto());

        return new Error("Failed to create shopping cart");
    }

    public async Task<Result<List<DtoShoppingCartSession>>> GetAll(CancellationToken cancellation)
    {
        var shoppingCarts = await context.ShoppingCarts
            .AsNoTracking()
            .Include(shoppingCart => shoppingCart.Details)
            .ToListAsync(cancellation);

        return new Success(shoppingCarts.Select(shoppingCart => shoppingCart.MapToDto()).ToList());
    }

    public async Task<Result<DtoShoppingCartSession>> GetById(Guid id, CancellationToken cancellation)
    {
        var shoppingCart = await context.ShoppingCarts
            .AsNoTracking()
            .Include(shoppingCart => shoppingCart.Details)
            .FirstOrDefaultAsync(shoppingCart => shoppingCart.Id == id, cancellation);

        if (shoppingCart is null)
            return new Error("Shopping cart not found", 404);

        return new Success(shoppingCart.MapToDto());
    }

    public async Task<Result<DtoShoppingCartSession>> Update(DtoShoppingCartSession shoppingCart, CancellationToken cancellation)
    {
        if (shoppingCart is null)
            return new Error("Shopping cart is required", 400);

        var existingShoppingCart = await context.ShoppingCarts
            .Include(shoppingCartItem => shoppingCartItem.Details)
            .FirstOrDefaultAsync(shoppingCartItem => shoppingCartItem.Id == shoppingCart.Id, cancellation);

        if (existingShoppingCart is null)
            return new Error("Shopping cart not found", 404);

        existingShoppingCart.CreateAt = shoppingCart.CreateAt == default ? existingShoppingCart.CreateAt : shoppingCart.CreateAt;

        existingShoppingCart.Details.Clear();
        foreach (var detail in shoppingCart.Details ?? [])
        {
            existingShoppingCart.Details.Add(new ShoppingCartDetailSessionModel
            {
                Id = detail.Id == Guid.Empty ? Guid.NewGuid() : detail.Id,
                CreateAt = existingShoppingCart.CreateAt,
                ProductSelected = detail.ProductSelected,
                ShoppingCartSessionId = existingShoppingCart.Id,
                ShoppingCart = existingShoppingCart
            });
        }

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(existingShoppingCart.MapToDto());

        return new Error("Failed to update shopping cart");
    }

    public async Task<Result> Delete(Guid id, CancellationToken cancellation)
    {
        var shoppingCart = await context.ShoppingCarts
            .Include(shoppingCart => shoppingCart.Details)
            .FirstOrDefaultAsync(shoppingCart => shoppingCart.Id == id, cancellation);

        if (shoppingCart is null)
            return new Error("Shopping cart not found", 404);

        context.ShoppingCarts.Remove(shoppingCart);

        if (await context.SaveChangesAsync(cancellation) > 0)
            return new Success(true);

        return new Error("Failed to delete shopping cart");
    }
}

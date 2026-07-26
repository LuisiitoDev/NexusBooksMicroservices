using NexusBooks.ShoppingCart.Application.Dtos;
using NexusBooks.ShoppingCart.Domain.Models;

namespace NexusBooks.ShoppingCart.Application.Extensions;

internal static class MapperExtensions
{
    extension(ShoppingCartSessionModel shoppingCart)
    {
        public DtoShoppingCartSession MapToDto()
        {
            return new DtoShoppingCartSession(
                shoppingCart.Id,
                shoppingCart.CreateAt,
                shoppingCart.Details.Select(detail => detail.MapToDto()).ToList());
        }
    }

    extension(ShoppingCartDetailSessionModel detail)
    {
        public DtoShoppingCartDetailSession MapToDto()
        {
            return new DtoShoppingCartDetailSession(detail.Id, detail.ProductSelected);
        }
    }

    extension(DtoShoppingCartSession shoppingCart)
    {
        public ShoppingCartSessionModel MapToModel()
        {
            var model = new ShoppingCartSessionModel
            {
                Id = shoppingCart.Id == Guid.Empty ? Guid.NewGuid() : shoppingCart.Id,
                CreateAt = shoppingCart.CreateAt == default ? DateTime.UtcNow : shoppingCart.CreateAt
            };

            model.Details = (shoppingCart.Details ?? []).Select(detail => new ShoppingCartDetailSessionModel
            {
                Id = detail.Id == Guid.Empty ? Guid.NewGuid() : detail.Id,
                CreateAt = model.CreateAt,
                ProductSelected = detail.ProductSelected,
                ShoppingCartSessionId = model.Id,
                ShoppingCart = model
            }).ToList();

            return model;
        }
    }
}

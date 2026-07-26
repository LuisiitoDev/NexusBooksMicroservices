namespace NexusBooks.ShoppingCart.Application.Dtos;

public record DtoShoppingCartSession(Guid Id, DateTime CreateAt, List<DtoShoppingCartDetailSession> Details);

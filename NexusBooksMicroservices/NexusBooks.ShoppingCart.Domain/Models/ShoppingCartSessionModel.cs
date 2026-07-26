namespace NexusBooks.ShoppingCart.Domain.Models;

public class ShoppingCartSessionModel
{
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public ICollection<ShoppingCartDetailSessionModel> Details { get; set; } = [];
}

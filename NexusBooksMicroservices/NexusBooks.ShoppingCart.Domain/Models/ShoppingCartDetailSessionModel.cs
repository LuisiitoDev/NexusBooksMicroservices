namespace NexusBooks.ShoppingCart.Domain.Models;

public class ShoppingCartDetailSessionModel
{
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public long ProductSelected { get; set; }
    public Guid ShoppingCartSessionId { get; set; }
    public required ShoppingCartSessionModel ShoppingCart { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace NexusBooks.Client.Models;

public sealed record AuthorDto(long Id, string Name, DateTime BirthDate);

public sealed record BookDto(long Id, string Title, string OverView, string ISBN, DateTime PublicationDate, List<long>? AuthorIds);

public sealed record ShoppingCartDetailDto(Guid Id, long ProductSelected);

public sealed record ShoppingCartSessionDto(Guid Id, DateTime CreateAt, List<ShoppingCartDetailDto> Details);

public sealed class AuthorFormModel
{
    public long Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; } = DateTime.Today;
}

public sealed class BookFormModel
{
    public long Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string OverView { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    public DateTime PublicationDate { get; set; } = DateTime.Today;

    public HashSet<long> SelectedAuthorIds { get; set; } = [];
}

public sealed class PurchaseFormModel
{
    [Range(1, long.MaxValue, ErrorMessage = "Select a book to purchase.")]
    public long SelectedBookId { get; set; }
}

namespace NexusBooks.Book.Domain.Models;

public class BookModel
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string OverView { get; set; }
    public required string ISBN { get; set; }
    public DateTime PublicationDate { get; set; }
    public ICollection<BookAuthorModel> Authors { get; set; } = [];
}

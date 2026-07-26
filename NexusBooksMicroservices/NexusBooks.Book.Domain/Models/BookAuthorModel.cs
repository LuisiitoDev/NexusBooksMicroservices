namespace NexusBooks.Book.Domain.Models;

public class BookAuthorModel
{
    public long BookId { get; set; }
    public required BookModel Book { get; set; }
    public long AuthorId { get; set; }
}

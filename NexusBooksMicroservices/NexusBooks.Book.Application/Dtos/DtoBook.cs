namespace NexusBooks.Book.Application.Dtos;

public record DtoBook(long Id, string Title, string OverView, string ISBN, DateTime PublicationDate, List<long>? AuthorIds);

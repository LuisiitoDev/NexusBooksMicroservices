using NexusBooks.Book.Application.Dtos;
using NexusBooks.Book.Domain.Models;

namespace NexusBooks.Book.Application.Extensions;

internal static class MapperExtensions
{
    extension(BookModel book)
    {
        public DtoBook MapToDto()
        {
            return new DtoBook(
                book.Id,
                book.Title,
                book.OverView,
                book.ISBN,
                book.PublicationDate,
                book.Authors.Select(author => author.AuthorId).ToList());
        }
    }

    extension(DtoBook book)
    {
        public BookModel MapToModel()
        {
            var model = new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                OverView = book.OverView,
                ISBN = book.ISBN,
                PublicationDate = book.PublicationDate
            };

            model.Authors = (book.AuthorIds ?? []).Distinct().Select(authorId => new BookAuthorModel
            {
                Book = model,
                AuthorId = authorId
            }).ToList();

            return model;
        }
    }
}

using NexusBooks.Author.Application.Dtos;
using NexusBooks.Author.Domain.Models;

namespace NexusBooks.Author.Application.Extensions;

internal static class MapperExtensions
{
    extension(AuthorModel author)
    {
        public DtoAuthor MapToDto()
        {
            return new DtoAuthor(author.Id, author.Name, author.BirthDate);
        }
    }

    extension (DtoAuthor author)
    {
        public AuthorModel MapToModel()
        {
            return new AuthorModel
            {
                Id = author.Id,
                Name = author.Name,
                BirthDate = author.BirthDate
            };
        }
    }
}

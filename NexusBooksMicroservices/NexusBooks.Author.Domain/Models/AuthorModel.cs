namespace NexusBooks.Author.Domain.Models
{
    public class AuthorModel
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateOnly CreateAt { get; set; }
    }
}

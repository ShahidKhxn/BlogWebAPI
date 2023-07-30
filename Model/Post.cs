using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebAPI.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public Author Author { get; set; }
        public Category Category { get; set; }
    }
}

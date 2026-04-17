using System;
namespace RentACar.Entities
{
    public class BlogPost
    {
        public int BlogPostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ImageUrl { get; set; }

        public bool IsApproved { get; set; }
    }

}
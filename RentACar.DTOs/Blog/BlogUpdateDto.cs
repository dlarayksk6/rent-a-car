using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RentACar.DTOs.Blog
{
    public class BlogUpdateDto
    {
        public int BlogPostId { get; set; }

      
        public string Title { get; set; }

     
        public string Content { get; set; }

        public string? AuthorId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ExistingImageUrl { get; set; }
        public bool IsApproved { get; set; }
    }
}
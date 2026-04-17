using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;




    namespace RentACar.DTOs.Blog
    {
        public class BlogCreateDto
        {
            
            public string Title { get; set; }

      
            public string Content { get; set; }

            public string? AuthorId { get; set; }
            public string? ImageUrl { get; set; }
            public bool IsApproved { get; set; }
        }
    
}
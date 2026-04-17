using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Entities
{
    // Entities/Brand.cs
    public class Brand
    {
       
        public int BrandId { get; set; }

        public string Name { get; set; }

        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.Brand
{
    public class BrandCreateDto
    {
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

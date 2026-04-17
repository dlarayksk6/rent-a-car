using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.Brand
{
    public class BrandUpdateDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public string? ExistingLogoUrl { get; set; }
        public bool IsActive { get; set; }
    }
}

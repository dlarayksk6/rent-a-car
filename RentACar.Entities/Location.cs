using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RentACar.Entities
{
    public class Location  // public eksik olabilir
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public bool IsPickup { get; set; }
        public bool IsDropoff { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
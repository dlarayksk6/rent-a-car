using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.Contact
{
    public class ContactUpdateDto
    {
        public int SiteContactId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? WhatsappNumber { get; set; }
        public string? WorkingHours { get; set; }
    }
}

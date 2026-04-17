using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Entities
{
    public class SiteContact
    {
        public int SiteContactId { get; set; }


        public string? Phone { get; set; }

        
        public string? Email { get; set; }

       
        public string? Address { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

  
        public string? Twitter { get; set; }
        public string? WhatsappNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? WorkingHours { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}



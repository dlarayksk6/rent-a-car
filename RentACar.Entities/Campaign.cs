using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Entities
{
    
        public class Campaign
        {
            public int CampaignId { get; set; }
            public string Title { get; set; }
            public string? Description { get; set; }
            public string? ImageUrl { get; set; }
            public bool IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}

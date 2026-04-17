using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.Campaign
{
    public class CampaignUpdateDto
    {
        public int CampaignId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? ExistingImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}

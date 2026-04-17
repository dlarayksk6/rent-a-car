using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.Contract
{
    public class ContractUpdateDto
    {
        public int ContractId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ContentText { get; set; }
        public string? PdfUrl { get; set; }
        public string? ExistingPdfUrl { get; set; }
        public bool IsActive { get; set; }
    }
}

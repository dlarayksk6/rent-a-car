using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Entities
{
    public class ContractDocument
    {
       
        public int ContractId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }  // Sayfa başı açıklama
        public string? ContentText { get; set; }  // Direkt yazı
        public string? PdfUrl { get; set; }       // PDF yolu
        public bool IsActive { get; set; } = true;
    }
}

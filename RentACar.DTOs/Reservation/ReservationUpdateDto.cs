using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace RentACar.DTOs.Reservation
{
    public class ReservationUpdateDto
    {
        public int ReservationId { get; set; }
        public int CarId { get; set; }
        public string? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
        public bool IsPaid { get; set; }
        public bool IsAdminBlocked { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
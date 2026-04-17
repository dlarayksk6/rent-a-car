using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.Reservation
{
    public class ReservationListDto
    {
        public int ReservationId { get; set; }
        public int CarId { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string PlateNumber { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public bool IsAdminBlocked { get; set; }
        public string? CarImageUrl { get; set; }
        public int CarYear { get; set; }
        public string? CarColor { get; set; }
        public string? CarCategory { get; set; }
        public string? CarFuelType { get; set; }
        public string? CarTransmission { get; set; }
        public decimal CarDailyPrice { get; set; }
        public string? PickupLocation { get; set; }
        public string? DropoffLocation { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using System;
using RentACar.Entities;
namespace RentACar.Entities
{

    public class Reservation
    {
        public int ReservationId { get; set; }
        public int CarId { get; set; }
        public string? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsAdminBlocked { get; set; } = false;
        public string Status { get; set; }
        public string Notes { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }

        public Car Car { get; set; }

    }

}


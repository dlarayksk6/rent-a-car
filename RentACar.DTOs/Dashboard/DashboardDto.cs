using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RentACar.DTOs.Reservation;

namespace RentACar.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int TotalCars { get; set; }
        public int TotalReservations { get; set; }
        public int ActiveReservations { get; set; }
        public int CompletedReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<ReservationListDto> RecentReservations { get; set; }
    }
}
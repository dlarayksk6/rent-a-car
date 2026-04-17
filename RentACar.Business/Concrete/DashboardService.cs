using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Dashboard;
using RentACar.DTOs.Reservation;

namespace RentACar.Business.Concrete
{
    public class DashboardService : IDashboardService
    {
        private readonly RentACarDbContext _context;

        public DashboardService(RentACarDbContext context)
        {
            _context = context;
        }

        public IDataResult<DashboardDto> GetDashboardData()
        {
            var recentReservations = _context.Reservations
                .Include(r => r.Car)
                .ToList()
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .Select(r => new ReservationListDto
                {
                    ReservationId = r.ReservationId,
                    CarId = r.CarId,
                    CarBrand = r.Car.Brand,
                    CarModel = r.Car.Model,
                    PlateNumber = r.Car.PlateNumber,
                    FullName = r.FullName,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    TotalPrice = r.TotalPrice,
                    Status = r.Status,
                    IsPaid = r.IsPaid,
                    CreatedAt = r.CreatedAt
                }).ToList();

            var dto = new DashboardDto
            {
                TotalCars = _context.Cars.Count(),
                TotalReservations = _context.Reservations.Count(),
                ActiveReservations = _context.Reservations.Count(r => r.Status == "Onaylı"),
                CompletedReservations = _context.Reservations.Count(r => r.Status == "Tamamlandı"),
                TotalRevenue = _context.Reservations
                    .Where(r => r.Status == "Onaylı" || r.Status == "Tamamlandı")
                    .Sum(r => r.TotalPrice),
                RecentReservations = recentReservations
            };

            return new SuccessDataResult<DashboardDto>(dto);
        }
    }
}
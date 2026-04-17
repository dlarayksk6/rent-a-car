using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IResult = RentACar.Core.Results.IResult;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Reservation;
using RentACar.Entities;

namespace RentACar.Business.Concrete
{
    public class ReservationService : IReservationService
    {
        private readonly RentACarDbContext _context;

        public ReservationService(RentACarDbContext context)
        {
            _context = context;
        }

        public IDataResult<List<ReservationListDto>> GetAll(string search = "", string status = "")
        {
            var query = _context.Reservations.Include(r => r.Car).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(r =>
                    r.FullName.Contains(search) ||
                    r.NationalId.Contains(search) ||
                    r.PhoneNumber.Contains(search) ||
                    r.Car.PlateNumber.Contains(search) ||
                    r.Car.Brand.Contains(search) ||
                    r.Car.Model.Contains(search));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            var result = query
                .OrderByDescending(r => r.CreatedAt)
                .ToList()              // ← önce çek
                .Select(r => ToDto(r)) // ← sonra map et
                .ToList();

            return new SuccessDataResult<List<ReservationListDto>>(result);
        }

        public IDataResult<ReservationListDto> GetById(int reservationId)
        {
            var r = _context.Reservations.Include(r => r.Car)
                .FirstOrDefault(r => r.ReservationId == reservationId);
            if (r == null) return new ErrorDataResult<ReservationListDto>("Rezervasyon bulunamadı.");
            return new SuccessDataResult<ReservationListDto>(ToDto(r));
        }

        public IDataResult<List<ReservationListDto>> GetByCarId(int carId)
        {
            var result = _context.Reservations
                .Include(r => r.Car)
                .Where(r => r.CarId == carId)
                .ToList()              // ← önce çek
                .Select(r => ToDto(r)) // ← sonra map et
                .ToList();
            return new SuccessDataResult<List<ReservationListDto>>(result);
        }

        public IResult Add(ReservationCreateDto dto)
        {
            var car = _context.Cars.FirstOrDefault(c => c.CarId == dto.CarId);
            if (car == null) return new ErrorResult("Araç bulunamadı.");

            // Saati StartDate ve EndDate'e göm
            if (!string.IsNullOrEmpty(dto.PickupTime) && TimeSpan.TryParse(dto.PickupTime, out var pickupTs))
                dto.StartDate = dto.StartDate.Date + pickupTs;
            else
                dto.StartDate = dto.StartDate.Date + new TimeSpan(10, 0, 0); // varsayılan 10:00

            if (!string.IsNullOrEmpty(dto.DropoffTime) && TimeSpan.TryParse(dto.DropoffTime, out var dropoffTs))
                dto.EndDate = dto.EndDate.Date + dropoffTs;
            else
                dto.EndDate = dto.EndDate.Date + new TimeSpan(18, 0, 0); // varsayılan 18:00

            if (dto.StartDate >= dto.EndDate)
                return new ErrorResult("Bitiş tarihi başlangıçtan sonra olmalı.");

            if (HasOverlap(dto.CarId, dto.StartDate, dto.EndDate))
                return new ErrorResult("Bu tarihler arasında araç zaten rezerve edilmiş.");

            // Gün hesabı: saat dahil, minimum 1 gün, yukarı yuvarla
            int days = Math.Max(1, (int)Math.Ceiling((dto.EndDate - dto.StartDate).TotalHours / 24.0));

            var reservation = new Reservation
            {
                CarId = dto.CarId,
                UserId = dto.UserId,
                FullName = dto.FullName ?? "Admin",
                PhoneNumber = dto.PhoneNumber ?? "-",
                NationalId = dto.NationalId ?? "-",
                StartDate = dto.StartDate,   // artık saat de var: 2025-06-10 10:00
                EndDate = dto.EndDate,     // artık saat de var: 2025-06-12 18:00
                TotalPrice = days * car.DailyPrice,
                Status = "Beklemede",
                IsPaid = false,
                IsAdminBlocked = false,
                PickupLocation = dto.PickupLocation ?? "",
                DropoffLocation = dto.DropoffLocation ?? "",
                Notes = dto.Notes ?? "",
                CreatedAt = DateTime.Now
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return new SuccessResult("Rezervasyon oluşturuldu.");
        }
        public IResult Update(ReservationUpdateDto dto)
        {
            var reservation = _context.Reservations.Find(dto.ReservationId);
            if (reservation == null) return new ErrorResult("Rezervasyon bulunamadı.");

            reservation.Status = dto.Status;
            reservation.IsPaid = dto.IsPaid;
            reservation.Notes = dto.Notes;

            _context.SaveChanges();
            return new SuccessResult("Rezervasyon güncellendi.");
        }

        public IResult Cancel(int reservationId)
        {
            var reservation = _context.Reservations.Find(reservationId);
            if (reservation == null) return new ErrorResult("Rezervasyon bulunamadı.");

            reservation.Status = "İptal";
            _context.SaveChanges();
            return new SuccessResult("Rezervasyon iptal edildi.");
        }

        public IResult Delete(int reservationId)
        {
            var reservation = _context.Reservations.Find(reservationId);
            if (reservation == null) return new ErrorResult("Rezervasyon bulunamadı.");

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return new SuccessResult("Rezervasyon silindi.");
        }
        public IResult BlockDates(int carId, DateTime startDate, DateTime endDate, string blockType = "Kiralama", string notes = "", decimal totalPrice = 0)
        {
            if (startDate >= endDate)
                return new ErrorResult("Bitiş tarihi başlangıçtan sonra olmalı.");

            if (HasOverlap(carId, startDate, endDate))
                return new ErrorResult("Bu tarih aralığı zaten rezerve edilmiş.");

            var reservation = new Reservation
            {
                CarId = carId,
                StartDate = startDate,
                EndDate = endDate,
                IsAdminBlocked = true,
                Status = blockType, // "Kiralama" veya "Bakım"
                FullName = "Admin",
                PhoneNumber = "-",
                NationalId = "-",
                
                PickupLocation = "-",   // ← ekle
                DropoffLocation = "-",
                TotalPrice = totalPrice,
                IsPaid = false,
                CreatedAt = DateTime.Now,
                Notes = string.IsNullOrEmpty(notes) ? $"Admin - {blockType}" : notes
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return new SuccessResult($"{blockType} bloğu oluşturuldu.");
        }

        private bool HasOverlap(int carId, DateTime start, DateTime end) =>
            _context.Reservations.Any(r =>
                r.CarId == carId &&
                r.Status != "İptal" &&
                (
                    (start >= r.StartDate && start < r.EndDate) ||
                    (end > r.StartDate && end <= r.EndDate) ||
                    (start <= r.StartDate && end >= r.EndDate)
                ));

        private ReservationListDto ToDto(Reservation r) => new ReservationListDto
        {
            ReservationId = r.ReservationId,
            CarId = r.CarId,
            CarBrand = r.Car?.Brand,
            CarModel = r.Car?.Model,
            PlateNumber = r.Car?.PlateNumber,
            FullName = r.FullName,
            PhoneNumber = r.PhoneNumber,
            NationalId = r.NationalId,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            TotalPrice = r.TotalPrice,
            Status = r.Status,
            IsPaid = r.IsPaid,
            IsAdminBlocked = r.IsAdminBlocked,
            Notes = r.Notes,
            CreatedAt = r.CreatedAt,
            CarImageUrl = r.Car?.ImageUrl,
            CarYear = r.Car?.Year ?? 0,
            CarColor = r.Car?.Color,
            CarCategory = r.Car?.Category,
            CarFuelType = r.Car?.FuelType,
            CarTransmission = r.Car?.Transmission,
            CarDailyPrice = r.Car?.DailyPrice ?? 0,
            PickupLocation = r.PickupLocation,
            DropoffLocation = r.DropoffLocation,
        };
    }
}
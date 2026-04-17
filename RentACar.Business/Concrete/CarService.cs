using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Car;

using RentACar.DTOs.Reservation;
using RentACar.Entities;
using IResult = RentACar.Core.Results.IResult;

namespace RentACar.Business.Concrete
{
    public class CarService : ICarService
    {
        private readonly RentACarDbContext _context;

        public CarService(RentACarDbContext context)
        {
            _context = context;
        }

        public IDataResult<List<CarListDto>> GetAll()
        {
            var cars = _context.Cars
                .Include(c => c.Reservations)
                .ToList()
                .Select(c => ToDto(c))
                .ToList();
            return new SuccessDataResult<List<CarListDto>>(cars);
        }

        public IDataResult<List<CarListDto>> GetAllActive()
        {
            var cars = _context.Cars
                .Where(c => c.Status == true)
                .OrderByDescending(c => c.CreatedAt)
                .ToList()
                .Select(c => ToDto(c))
                .ToList();
            return new SuccessDataResult<List<CarListDto>>(cars);
        }

        public IDataResult<CarListDto> GetById(int carId)
        {
            var car = _context.Cars
                .Include(c => c.Reservations)
                .FirstOrDefault(c => c.CarId == carId);
            if (car == null)
                return new ErrorDataResult<CarListDto>("Araç bulunamadı.");
            return new SuccessDataResult<CarListDto>(ToDto(car));
        }

        public IDataResult<List<CarListDto>> GetFiltered(
          string[] category, string[] fuel, string[] transmission,
          string[] seats, int? minYear, int? maxYear,
          decimal? minPrice, decimal? maxPrice,
          int? maxMinAge, int? maxMinLicense,
          string[] colors, string[] brands,
          string[] bodyTypes, int? minLuggage,
          int? minHorsePower,
          bool? hasAC, bool? hasBluetooth,
          bool? hasNavigation, bool? hasBackCamera,
          bool? hasSunroof, bool? hasHeatedSeats,
          bool? hasCruiseControl, bool? hasParkingSensor,
          bool? hasChildSeat, bool? hasUSBPort)
        {
            var query = _context.Cars.AsQueryable();

            if (category?.Length > 0)
                query = query.Where(c => category.Contains(c.Category));
            if (fuel?.Length > 0)
            {
                var lowerFuel = fuel.Select(f => f.ToLower()).ToArray();
                query = query.Where(c => lowerFuel.Contains(c.FuelType.ToLower()));
            }
            if (transmission?.Length > 0)
            {
                var lowerTrans = transmission.Select(t => t.ToLower()).ToArray();
                query = query.Where(c => lowerTrans.Contains(c.Transmission.ToLower()));
            }
            if (seats?.Length > 0)
            {
                var seatInts = seats.Select(s => int.TryParse(s, out var n) ? n : 0).ToArray();
                query = query.Where(c => seatInts.Contains(c.Seats));
            }
            if (minYear.HasValue) query = query.Where(c => c.Year >= minYear.Value);
            if (maxYear.HasValue) query = query.Where(c => c.Year <= maxYear.Value);
            if (minPrice.HasValue) query = query.Where(c => c.DailyPrice >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(c => c.DailyPrice <= maxPrice.Value);
            if (maxMinAge.HasValue) query = query.Where(c => c.MinAge <= maxMinAge.Value);
            if (maxMinLicense.HasValue) query = query.Where(c => c.MinDriverLicenseYear <= maxMinLicense.Value);

            // Yeni filtreler
            if (colors?.Length > 0)
            {
                var lowerColors = colors.Select(x => x.ToLower()).ToArray();
                query = query.Where(c => lowerColors.Contains(c.Color.ToLower()));
            }
            if (brands?.Length > 0)
            {
                var lowerBrands = brands.Select(x => x.ToLower()).ToArray();
                query = query.Where(c => lowerBrands.Contains(c.Brand.ToLower()));
            }
            if (bodyTypes?.Length > 0)
            {
                var lowerBody = bodyTypes.Select(x => x.ToLower()).ToArray();
                query = query.Where(c => c.BodyType != null && lowerBody.Contains(c.BodyType.ToLower()));
            }
            if (minLuggage.HasValue) query = query.Where(c => c.LuggageCapacity >= minLuggage.Value);
            if (minHorsePower.HasValue) query = query.Where(c => c.HorsePower >= minHorsePower.Value);

            if (hasAC == true) query = query.Where(c => c.HasAirConditioning);
            if (hasBluetooth == true) query = query.Where(c => c.HasBluetooth);
            if (hasNavigation == true) query = query.Where(c => c.HasNavigation);
            if (hasBackCamera == true) query = query.Where(c => c.HasBackCamera);
            if (hasSunroof == true) query = query.Where(c => c.HasSunroof);
            if (hasHeatedSeats == true) query = query.Where(c => c.HasHeatedSeats);
            if (hasCruiseControl == true) query = query.Where(c => c.HasCruiseControl);
            if (hasParkingSensor == true) query = query.Where(c => c.HasParkingSensor);
            if (hasChildSeat == true) query = query.Where(c => c.HasChildSeat);
            if (hasUSBPort == true) query = query.Where(c => c.HasUSBPort);

            var result = query.ToList().Select(c => ToDto(c)).ToList();
            return new SuccessDataResult<List<CarListDto>>(result);
        }

        public IResult Add(CarCreateDto dto, string webRootPath)
        {
            var car = new Car
            {
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                Color = dto.Color,
                Category = dto.Category,
                PlateNumber = dto.PlateNumber,
                DailyPrice = dto.DailyPrice,
                Transmission = dto.Transmission,
                FuelType = dto.FuelType,
                Seats = dto.Seats,
                MinAge = dto.MinAge,
                MinDriverLicenseYear = dto.MinDriverLicenseYear,
                Status = dto.Status,
                Description = dto.Description,
                BodyType = dto.BodyType,
                EngineCC = dto.EngineCC,
                HorsePower = dto.HorsePower,
                LuggageCapacity = dto.LuggageCapacity,
                DoorCount = dto.DoorCount,
                DriveType = dto.DriveType,
                Mileage = dto.Mileage,
                MileageLimit = dto.MileageLimit,
                ExtraKmPrice = dto.ExtraKmPrice,
                DepositAmount = dto.DepositAmount,
                MaxDriverAge = dto.MaxDriverAge,
                RequiresCreditCard = dto.RequiresCreditCard,
                MaxRentalDays = dto.MaxRentalDays,
                MinRentalDays = dto.MinRentalDays,
                HasAirConditioning = dto.HasAirConditioning,
                HasBluetooth = dto.HasBluetooth,
                HasNavigation = dto.HasNavigation,
                HasBackCamera = dto.HasBackCamera,
                HasSunroof = dto.HasSunroof,
                HasHeatedSeats = dto.HasHeatedSeats,
                HasCruiseControl = dto.HasCruiseControl,
                HasParkingSensor = dto.HasParkingSensor,
                IsSmokingAllowed = dto.IsSmokingAllowed,
                HasChildSeat = dto.HasChildSeat,
                HasUSBPort = dto.HasUSBPort,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.Now
            };

            _context.Cars.Add(car);
            _context.SaveChanges();
            return new SuccessResult("Araç başarıyla eklendi.");
        }

        public IResult Update(CarUpdateDto dto, string webRootPath)
        {
            var car = _context.Cars.Find(dto.CarId);
            if (car == null) return new ErrorResult("Araç bulunamadı.");

            car.Brand = dto.Brand;
            car.Model = dto.Model;
            car.Year = dto.Year;
            car.Color = dto.Color;
            car.Category = dto.Category;
            car.PlateNumber = dto.PlateNumber;
            car.DailyPrice = dto.DailyPrice;
            car.Transmission = dto.Transmission;
            car.FuelType = dto.FuelType;
            car.Seats = dto.Seats;
            car.MinAge = dto.MinAge;
            car.MinDriverLicenseYear = dto.MinDriverLicenseYear;
            car.Status = dto.Status;
            car.Description = dto.Description;
            car.BodyType = dto.BodyType;
            car.EngineCC = dto.EngineCC;
            car.HorsePower = dto.HorsePower;
            car.LuggageCapacity = dto.LuggageCapacity;
            car.DoorCount = dto.DoorCount;
            car.DriveType = dto.DriveType;
            car.Mileage = dto.Mileage;
            car.MileageLimit = dto.MileageLimit;
            car.ExtraKmPrice = dto.ExtraKmPrice;
            car.DepositAmount = dto.DepositAmount;
            car.MaxDriverAge = dto.MaxDriverAge;
            car.RequiresCreditCard = dto.RequiresCreditCard;
            car.MaxRentalDays = dto.MaxRentalDays;
            car.MinRentalDays = dto.MinRentalDays;
            car.HasAirConditioning = dto.HasAirConditioning;
            car.HasBluetooth = dto.HasBluetooth;
            car.HasNavigation = dto.HasNavigation;
            car.HasBackCamera = dto.HasBackCamera;
            car.HasSunroof = dto.HasSunroof;
            car.HasHeatedSeats = dto.HasHeatedSeats;
            car.HasCruiseControl = dto.HasCruiseControl;
            car.HasParkingSensor = dto.HasParkingSensor;
            car.IsSmokingAllowed = dto.IsSmokingAllowed;
            car.HasChildSeat = dto.HasChildSeat;
            car.HasUSBPort = dto.HasUSBPort;
            car.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.ImageUrl))
            {
                DeleteImage(car.ImageUrl, webRootPath);
                car.ImageUrl = dto.ImageUrl;
            }

            _context.SaveChanges();
            return new SuccessResult("Araç güncellendi.");
        }

        public IResult Delete(int carId, string webRootPath)
        {
            var car = _context.Cars.Find(carId);
            if (car == null) return new ErrorResult("Araç bulunamadı.");

            DeleteImage(car.ImageUrl, webRootPath);
            _context.Cars.Remove(car);
            _context.SaveChanges();
            return new SuccessResult("Araç silindi.");
        }

        private void DeleteImage(string? imageUrl, string webRootPath)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;
            var path = Path.Combine(webRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(path)) File.Delete(path);
        }

        private CarListDto ToDto(Car c) => new CarListDto
        {
            CarId = c.CarId,
            Brand = c.Brand,
            Model = c.Model,
            Year = c.Year,
            Color = c.Color,
            Category = c.Category,
            PlateNumber = c.PlateNumber,
            DailyPrice = c.DailyPrice,
            Transmission = c.Transmission,
            FuelType = c.FuelType,
            Seats = c.Seats,
            MinAge = c.MinAge,
            MinDriverLicenseYear = c.MinDriverLicenseYear,
            Status = c.Status,
            Description = c.Description,
            BodyType = c.BodyType,
            EngineCC = c.EngineCC,
            HorsePower = c.HorsePower,
            LuggageCapacity = c.LuggageCapacity,
            DoorCount = c.DoorCount,
            DriveType = c.DriveType,
            Mileage = c.Mileage,
            MileageLimit = c.MileageLimit,
            ExtraKmPrice = c.ExtraKmPrice,
            DepositAmount = c.DepositAmount,
            MaxDriverAge = c.MaxDriverAge,
            RequiresCreditCard = c.RequiresCreditCard,
            MaxRentalDays = c.MaxRentalDays,
            MinRentalDays = c.MinRentalDays,
            HasAirConditioning = c.HasAirConditioning,
            HasBluetooth = c.HasBluetooth,
            HasNavigation = c.HasNavigation,
            HasBackCamera = c.HasBackCamera,
            HasSunroof = c.HasSunroof,
            HasHeatedSeats = c.HasHeatedSeats,
            HasCruiseControl = c.HasCruiseControl,
            HasParkingSensor = c.HasParkingSensor,
            IsSmokingAllowed = c.IsSmokingAllowed,
            HasChildSeat = c.HasChildSeat,
            HasUSBPort = c.HasUSBPort,
            ImageUrl = c.ImageUrl,
            CreatedAt = c.CreatedAt,
            Reservations = c.Reservations?.Select(r => new ReservationListDto
            {
                ReservationId = r.ReservationId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status,
                IsAdminBlocked = r.IsAdminBlocked
            }).ToList() ?? new()
        };
    }
}
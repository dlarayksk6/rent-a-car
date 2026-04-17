using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace RentACar.DTOs.Car
{
    public class CarCreateDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string Category { get; set; }
        public string PlateNumber { get; set; }
        public decimal DailyPrice { get; set; }
        public string Transmission { get; set; }
        public string FuelType { get; set; }
        public int Seats { get; set; }
        public int MinAge { get; set; }
        public int MinDriverLicenseYear { get; set; }
        public decimal? DepositAmount { get; set; }
        public decimal? ExtraKmPrice { get; set; }
        public int? MaxDriverAge { get; set; }
        public int? MinRentalDays { get; set; }
        public int? MaxRentalDays { get; set; }
        public string? Description { get; set; }
        public string? BodyType { get; set; }
        public int? EngineCC { get; set; }
        public int? HorsePower { get; set; }
        public int? LuggageCapacity { get; set; }
        public int? DoorCount { get; set; }
        public string? DriveType { get; set; }
        public int? Mileage { get; set; }
        public int? MileageLimit { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBluetooth { get; set; }
        public bool HasNavigation { get; set; }
        public bool HasBackCamera { get; set; }
        public bool HasSunroof { get; set; }
        public bool HasHeatedSeats { get; set; }
        public bool HasCruiseControl { get; set; }
        public bool HasParkingSensor { get; set; }
        public bool IsSmokingAllowed { get; set; }
        public bool HasChildSeat { get; set; }
        public bool HasUSBPort { get; set; }
        public bool RequiresCreditCard { get; set; }
        public bool Status { get; set; } = true;
        public string? ImageUrl { get; set; }
    }
}
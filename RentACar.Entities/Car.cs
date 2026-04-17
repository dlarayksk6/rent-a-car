namespace RentACar.Entities
{
    public class Car
    {
        public int CarId { get; set; }

        // Temel Bilgiler
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        public string? Category { get; set; }
        public string? PlateNumber { get; set; }
        public bool Status { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal DailyPrice { get; set; }

        // Teknik Özellikler
        public string? Transmission { get; set; }
        public string? FuelType { get; set; }
        public int Seats { get; set; }
        public string? BodyType { get; set; }
        public int? EngineCC { get; set; }
        public int? HorsePower { get; set; }
        public int? LuggageCapacity { get; set; }
        public int? DoorCount { get; set; }
        public string? DriveType { get; set; }
        public int? Mileage { get; set; }
        public int? MileageLimit { get; set; }

        // Ekstra Ücretler
        public decimal? ExtraKmPrice { get; set; }
        public decimal? DepositAmount { get; set; }

        // Özellikler (bool)
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

        // Kiralama Koşulları
        public int MinAge { get; set; }
        public int MinDriverLicenseYear { get; set; }
        public int? MaxDriverAge { get; set; }
        public bool RequiresCreditCard { get; set; }
        public int? MaxRentalDays { get; set; }
        public int? MinRentalDays { get; set; }

        // Tarihler
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}

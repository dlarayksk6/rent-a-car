using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.Car;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Personel")]
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly IWebHostEnvironment _env;

        public CarsController(ICarService carService, IWebHostEnvironment env)
        {
            _carService = carService;
            _env = env;
        }

        [PermissionAuthorize("Cars.View")]
        public IActionResult Index()
        {
            var result = _carService.GetAll();
            return View(result.Data);
        }

        [HttpGet]
        [PermissionAuthorize("Cars.Manage")]
        public IActionResult Create()
        {
            return View(new CarCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Cars.Manage")]
        public async Task<IActionResult> Create(CarCreateDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View(dto);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "cars");
                Directory.CreateDirectory(folder);
                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
                await imageFile.CopyToAsync(stream);
                dto.ImageUrl = "/uploads/cars/" + fileName;
            }

            var result = _carService.Add(dto, _env.WebRootPath);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            if (result.Success)
                return RedirectToAction(nameof(Index));

            return View(dto);
        }

        [HttpGet]
        [PermissionAuthorize("Cars.Manage")]
        public IActionResult Edit(int id)
        {
            var result = _carService.GetById(id);
            if (!result.Success) return NotFound();
            var c = result.Data;

            var dto = new CarUpdateDto
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
                ExistingImageUrl = c.ImageUrl
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Cars.Manage")]
        public async Task<IActionResult> Edit(CarUpdateDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View(dto);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "cars");
                Directory.CreateDirectory(folder);
                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
                await imageFile.CopyToAsync(stream);
                dto.ImageUrl = "/uploads/cars/" + fileName;
            }

            var result = _carService.Update(dto, _env.WebRootPath);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            if (result.Success)
                return RedirectToAction(nameof(Index));

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Cars.Delete")]
        public IActionResult Delete(int id)
        {
            var result = _carService.Delete(id, _env.WebRootPath);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

     

    }
}
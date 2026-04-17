using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Business.Abstract;
using RentACar.Business.Concrete;
using RentACar.DTOs.Car;
using RentACar.DTOs.Reservation;
using RentACar.Entities;

namespace RentACar.WebUI.Controllers
{
    public class SiteController : Controller
    {
     
        
            private readonly ICarService _carService;
            private readonly IBlogService _blogService;
            private readonly ISiteContactService _siteContactService;
            private readonly IAboutService _aboutService;
            private readonly IReservationService _reservationService;
            private readonly ILocationService _locationService;
        private readonly ICampaignService _campaignService;
        private readonly IContractService _contractService;
        private readonly IBrandService _brandService;

        public SiteController(
                ICarService carService,
                IBlogService blogService,
                ISiteContactService siteContactService,
                IAboutService aboutService,
                IBrandService brandService,
                IReservationService reservationService,
                ILocationService locationService,
                ICampaignService campaignService, IContractService contractService)
            {
                _carService = carService;
                _blogService = blogService;
                _siteContactService = siteContactService;
                _aboutService = aboutService;
                _reservationService = reservationService;
                _locationService = locationService;
            _campaignService = campaignService;
            _contractService = contractService;
            _brandService = brandService;
        }

            public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var contact = _siteContactService.Get();
            ViewBag.SiteContact = contact.Success ? contact.Data : null;
        }

 

       
        public IActionResult CarDetail(int id)
        {
            var result = _carService.GetById(id);
            if (!result.Success) return NotFound();
            return View(result.Data);
        }

      

        public IActionResult Contact()
        {
            var result = _siteContactService.Get();
            return View(result.Success ? result.Data : new SiteContact());
        }
        public IActionResult BlogDetails(int id)
        {
            var result = _blogService.GetById(id);
            if (!result.Success) return NotFound();
            return View(result.Data);
        }
        public IActionResult Blog()
        {
            var result = _blogService.GetAllApproved();
            return View(result.Data);
        }

        public IActionResult About()
        {
            var result = _aboutService.Get();
            return View(result.Success ? result.Data : new AboutContent());
        }
        public IActionResult Index()
        {
            var result = _blogService.GetAllApproved();
            var about = _aboutService.Get();
            ViewBag.About = about.Success ? about.Data : null;

            // Araç Bul formu için konumlar
            ViewBag.PickupLocations = _locationService.GetPickupLocations().Data;
            ViewBag.DropoffLocations = _locationService.GetDropoffLocations().Data;
            // Mevcut arabalar listesinden distinct markaları al
            ViewBag.Brands = _brandService.GetActive().Data;
            return View(result.Data);
        }

        public IActionResult Cars(DateTime? startDate = null, DateTime? endDate = null, string category = null)
        {
            var result = _carService.GetAllActive();
            var cars = result.Data;

            // Tarihe göre müsait araçları filtrele
            if (startDate.HasValue && endDate.HasValue)
            {
                var reservations = _reservationService.GetAll().Data
                    .Where(r => r.Status != "İptal")
                    .ToList();

                var busyCarIds = reservations
                    .Where(r => r.StartDate.Date <= endDate.Value.Date && r.EndDate.Date >= startDate.Value.Date)
                    .Select(r => r.CarId)
                    .Distinct()
                    .ToHashSet();

                cars = cars.Where(c => !busyCarIds.Contains(c.CarId)).ToList();

                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                ViewBag.StartDateDisplay = startDate.Value.ToString("dd.MM.yyyy");
                ViewBag.EndDateDisplay = endDate.Value.ToString("dd.MM.yyyy");
            }

            // Kategori filtresi
            if (!string.IsNullOrEmpty(category))
            {
                cars = cars.Where(c => c.Category == category).ToList();
                ViewBag.SelectedCategory = category;
            }

            return View(cars);
        }

        [HttpGet]
  
        public IActionResult FilterCars(
    string[] category, string[] fuel, string[] transmission,
    string[] seats, int? minYear, int? maxYear,
    decimal? minPrice, decimal? maxPrice,
    int? maxMinAge, int? maxMinLicense,
    string[] colors, string[] brands, string[] bodyTypes,
    int? minLuggage, int? minHorsePower,
    bool? hasAC, bool? hasBluetooth, bool? hasNavigation,
    bool? hasBackCamera, bool? hasSunroof, bool? hasHeatedSeats,
    bool? hasCruiseControl, bool? hasParkingSensor,
    bool? hasChildSeat, bool? hasUSBPort)
        {
            var result = _carService.GetFiltered(
                category, fuel, transmission, seats,
                minYear, maxYear, minPrice, maxPrice,
                maxMinAge, maxMinLicense,
                colors, brands, bodyTypes, minLuggage, minHorsePower,
                hasAC, hasBluetooth, hasNavigation, hasBackCamera,
                hasSunroof, hasHeatedSeats, hasCruiseControl,
                hasParkingSensor, hasChildSeat, hasUSBPort);

            return PartialView("_CarListPartial", result.Data);
        }
        [HttpGet]
        public IActionResult CreateReservation(int carId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var carResult = _carService.GetById(carId);
            if (!carResult.Success) return NotFound();

            var car = carResult.Data;

            ViewBag.Car = car;
            ViewBag.PickupLocations = _locationService.GetPickupLocations().Data;
            ViewBag.DropoffLocations = _locationService.GetDropoffLocations().Data;

            // Dolu tarihleri al
            var reservations = _reservationService.GetByCarId(carId);
            if (reservations.Success)
            {
                var bookedDates = reservations.Data
                    .Where(r => r.Status != "İptal")
                    .Select(r => new { start = r.StartDate.ToString("yyyy-MM-dd"), end = r.EndDate.AddDays(1).ToString("yyyy-MM-dd") })
                    .ToList();
                ViewBag.BookedDates = System.Text.Json.JsonSerializer.Serialize(bookedDates);
            }

            var dto = new ReservationCreateDto { CarId = carId };

            if (User.Identity.IsAuthenticated)
                dto.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return View(dto);
        }

        [HttpPost]
        public IActionResult CreateReservation(ReservationCreateDto dto)
        {
            if (!string.IsNullOrEmpty(dto.PickupTime) && TimeSpan.TryParse(dto.PickupTime, out var pickupTs))
                dto.StartDate = dto.StartDate.Date + pickupTs;

            if (!string.IsNullOrEmpty(dto.DropoffTime) && TimeSpan.TryParse(dto.DropoffTime, out var dropoffTs))
                dto.EndDate = dto.EndDate.Date + dropoffTs;
            if (!ModelState.IsValid)
            {
                var carResult = _carService.GetById(dto.CarId);
                var car = carResult.Data;
                int days = (dto.EndDate - dto.StartDate).Days + 1;

                var reservations = _reservationService.GetByCarId(dto.CarId);
                ViewBag.BookedDates = reservations.Success
                    ? System.Text.Json.JsonSerializer.Serialize(
                        reservations.Data
                            .Where(r => r.Status != "İptal")
                            .Select(r => new { start = r.StartDate.ToString("yyyy-MM-dd"), end = r.EndDate.AddDays(1).ToString("yyyy-MM-dd") })
                            .ToList())
                    : "[]";

                ViewBag.Car = car;
                ViewBag.PickupLocations = _locationService.GetPickupLocations().Data;
                ViewBag.DropoffLocations = _locationService.GetDropoffLocations().Data;
                ViewBag.TotalPrice = days * car.DailyPrice;
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View(dto);
            }

            if (User.Identity.IsAuthenticated)
                dto.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var result = _reservationService.Add(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            if (result.Success)
                return RedirectToAction("ReservationConfirm");

            return RedirectToAction("CarDetail", new { id = dto.CarId });
        }
        public IActionResult ReservationConfirm()
        {
            return View();
        }
        public IActionResult Campaigns()
        {
            var result = _campaignService.GetAll();
            return View(result.Data);
        }

        public IActionResult Contracts()
        {
            var result = _contractService.GetActive();
            return View(result.Data);
        }
    }
}
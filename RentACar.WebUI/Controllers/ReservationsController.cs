using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.Reservation;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Personel")]
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ICarService _carService;

        public ReservationsController(IReservationService reservationService, ICarService carService)
        {
            _reservationService = reservationService;
            _carService = carService;
        }
      

        [PermissionAuthorize("Reservations.View")]
        public IActionResult Index(string search = "", string status = "")
        {
            var result = _reservationService.GetAll(search, status);
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Statuses = new List<string> { "Beklemede", "Onaylı", "Kapora Alındı", "Ödendi", "Tamamlandı", "İptal" };
            return View(result.Data);
        }

       
        [PermissionAuthorize("Reservations.View")]
        public IActionResult Calendar(int carId)
        {
            var carResult = _carService.GetById(carId);
            if (!carResult.Success) return NotFound();

            var reservations = _reservationService.GetByCarId(carId);
            var car = carResult.Data;
            car.Reservations = reservations.Success ? reservations.Data : new List<ReservationListDto>();

            return View(car);
        }

        [PermissionAuthorize("Reservations.View")]
        public IActionResult Details(int id)
        {
            var result = _reservationService.GetById(id);
            if (!result.Success) return NotFound();
            return View(result.Data);
        }

     

        [HttpPost]
        [PermissionAuthorize("Reservations.Manage")]
        public IActionResult BlockDates(int carId, DateTime startDate, DateTime endDate, string notes = "")
        {
            var result = _reservationService.BlockDates(carId, startDate, endDate, notes);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Reservations.Manage")]
        public IActionResult Create(ReservationCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return RedirectToAction("Index");
            }

            var result = _reservationService.Add(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
        [HttpPost]
        [PermissionAuthorize("Reservations.Approve")]
        public IActionResult UpdateStatus(int id, string status)
        {
            var existing = _reservationService.GetById(id);
            if (!existing.Success) return NotFound();

            var dto = new ReservationUpdateDto
            {
                ReservationId = id,
                Status = status,
                IsPaid = status == "Ödendi" || status == "Tamamlandı" || status == "Kapora Alındı",
                Notes = existing.Data.Notes
            };

            var result = _reservationService.Update(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return Ok();
        }
        [HttpPost]
        [PermissionAuthorize("Reservations.Delete")]
        public IActionResult Delete(int id)
        {
            var result = _reservationService.Delete(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [PermissionAuthorize("Reservations.Cancel")]
        public IActionResult Cancel(int id)
        {
            var result = _reservationService.Cancel(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
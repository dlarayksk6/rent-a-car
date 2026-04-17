using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.WebUI.Attributes;

[Authorize(Roles = "Admin, Personel")]
public class DashboardController : Controller
{
    private readonly ICarService _carService;
    private readonly IReservationService _reservationService;

    public DashboardController(ICarService carService, IReservationService reservationService)
    {
        _carService = carService;
        _reservationService = reservationService;
    }

    public IActionResult Index(string category = "A")
    {
        var result = _carService.GetAll();
        var cars = result.Data.Where(c => c.Category == category).ToList();
        ViewBag.SelectedCategory = category;
        ViewBag.Categories = new List<string> { "A", "B", "C", "D", "E" };
        return View(cars);
    }

    [HttpGet]
    [PermissionAuthorize("Reservations.Manage")]
    public IActionResult BlockDates(int carId)
    {
        var carResult = _carService.GetById(carId);
        if (!carResult.Success) return RedirectToAction("Index");

        var reservations = _reservationService.GetByCarId(carId);
        if (reservations.Success)
        {
            var bookedDates = reservations.Data
                .Where(r => r.Status != "İptal")
                .Select(r => new { start = r.StartDate.ToString("yyyy-MM-dd"), end = r.EndDate.AddDays(1).ToString("yyyy-MM-dd") })
                .ToList();
            ViewBag.BookedDates = System.Text.Json.JsonSerializer.Serialize(bookedDates);
        }
        else
        {
            ViewBag.BookedDates = "[]";
        }
        ViewBag.Car = carResult.Data;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize("Reservations.Manage")]
    public IActionResult BlockDates(
        int carId,
        DateTime startDate, string startTime,
        DateTime endDate, string endTime,
        string blockType, string notes = "")
    {
        if (TimeSpan.TryParse(startTime, out var sts)) startDate = startDate.Date + sts;
        if (TimeSpan.TryParse(endTime, out var ets)) endDate = endDate.Date + ets;

        var status = blockType == "Bakim" ? "Bakım" : "Kiralama";
        decimal totalPrice = 0;
        if (blockType == "Kiralama")
        {
            var car = _carService.GetById(carId);
            if (car.Success)
            {
                int days = Math.Max(1, (int)Math.Ceiling((endDate - startDate).TotalHours / 24.0));
                totalPrice = days * car.Data.DailyPrice;
            }
        }

        var result = _reservationService.BlockDates(carId, startDate, endDate, status, notes, totalPrice);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction("Index");
    }
}
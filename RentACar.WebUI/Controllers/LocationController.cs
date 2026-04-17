using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.Location;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Personel")]

    public class LocationsController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [PermissionAuthorize("Locations.View")]
        public IActionResult Index()
        {
            var result = _locationService.GetAll();
            return View(result.Data);
        }

        [HttpGet]
        [PermissionAuthorize("Locations.Manage")]
        public IActionResult Create()
        {
            return View(new LocationCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Locations.Manage")]
        public IActionResult Create(LocationCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return View(dto);
            }
            var result = _locationService.Add(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        [PermissionAuthorize("Locations.Manage")]
        public IActionResult Edit(int id)
        {
            var result = _locationService.GetById(id);
            if (!result.Success) return NotFound();

            var dto = new LocationUpdateDto
            {
                LocationId = result.Data.LocationId,
                Name = result.Data.Name,
                Address = result.Data.Address,
                IsPickup = result.Data.IsPickup,
                IsDropoff = result.Data.IsDropoff,
                IsActive = result.Data.IsActive
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Locations.Manage")]
        public IActionResult Edit(LocationUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return View(dto);
            }
            var result = _locationService.Update(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Locations.Delete")]
        public IActionResult Delete(int id)
        {
            var result = _locationService.Delete(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
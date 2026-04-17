using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.Brand;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize]
   
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IWebHostEnvironment _env;

        public BrandsController(IBrandService brandService, IWebHostEnvironment env)
        {
            _brandService = brandService;
            _env = env;
        }
        [PermissionAuthorize("Brands.View")]
        public IActionResult Index()
        {
            var brands = _brandService.GetAll().Data;
            return View(brands);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Brands.Manage")]
        public IActionResult Create(BrandCreateDto dto, IFormFile? logoFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return RedirectToAction("Index");
            }
            if (logoFile != null)
                dto.LogoUrl = SaveLogo(logoFile, dto.Name);

            var result = _brandService.Add(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Brands.Manage")]
        public IActionResult Edit(BrandUpdateDto dto, IFormFile? logoFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return RedirectToAction("Index");
            }
            if (logoFile != null)
                dto.LogoUrl = SaveLogo(logoFile, dto.Name);

            var result = _brandService.Update(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Brands.Delete")]
        public IActionResult Delete(int id)
        {
            var result = _brandService.Delete(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [PermissionAuthorize("Brands.Manage")]
        public IActionResult ToggleStatus(int id)
        {
            _brandService.ToggleStatus(id);
            return Json(new { success = true });
        }

        private string SaveLogo(IFormFile file, string brandName)
        {
            var folder = Path.Combine(_env.WebRootPath, "quarter/images/brands");
            Directory.CreateDirectory(folder);
            var fileName = brandName.ToLower().Replace(" ", "") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);
            return $"/quarter/images/brands/{fileName}";
        }
    }
}
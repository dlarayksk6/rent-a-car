using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.Campaign;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize]
  
    public class CampaignsController : Controller
    {
        private readonly ICampaignService _campaignService;
        private readonly IWebHostEnvironment _env;

        public CampaignsController(ICampaignService campaignService, IWebHostEnvironment env)
        {
            _campaignService = campaignService;
            _env = env;
        }

        [PermissionAuthorize("Campaigns.View")]
        public IActionResult Index()
        {
            var result = _campaignService.GetAll();
            return View(result.Data);
        }

        [HttpGet]
        [PermissionAuthorize("Campaigns.Manage")]
        public IActionResult Create() => View(new CampaignCreateDto());

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Campaigns.Manage")]
        public IActionResult Create(CampaignCreateDto dto, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return View(dto);
            }
            if (ImageFile != null)
                dto.ImageUrl = SaveImage(ImageFile);

            var result = _campaignService.Add(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        [PermissionAuthorize("Campaigns.Manage")]
        public IActionResult Edit(int id)
        {
            var result = _campaignService.GetById(id);
            if (!result.Success) return NotFound();

            var dto = new CampaignUpdateDto
            {
                CampaignId = result.Data.CampaignId,
                Title = result.Data.Title,
                Description = result.Data.Description,
                IsActive = result.Data.IsActive,
                ExistingImageUrl = result.Data.ImageUrl
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Campaigns.Manage")]
        public IActionResult Edit(CampaignUpdateDto dto, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return View(dto);
            }
            if (ImageFile != null)
            {
                DeleteImage(dto.ExistingImageUrl);
                dto.ImageUrl = SaveImage(ImageFile);
            }
            var result = _campaignService.Update(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [PermissionAuthorize("Campaigns.Delete")]
        public IActionResult Delete(int id)
        {
            var camp = _campaignService.GetById(id);
            if (camp.Success) DeleteImage(camp.Data.ImageUrl);
            var result = _campaignService.Delete(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [PermissionAuthorize("Campaigns.Manage")]
        public IActionResult ToggleStatus(int id)
        {
            var result = _campaignService.GetById(id);
            if (!result.Success) return NotFound();
            var dto = new CampaignUpdateDto
            {
                CampaignId = result.Data.CampaignId,
                Title = result.Data.Title,
                Description = result.Data.Description,
                IsActive = !result.Data.IsActive,
                ExistingImageUrl = result.Data.ImageUrl
            };
            _campaignService.Update(dto);
            return Ok();
        }

        private string SaveImage(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "images", "campaigns");
            Directory.CreateDirectory(folder);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folder, fileName);
            using var stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            return "/images/campaigns/" + fileName;
        }

        private void DeleteImage(string? url)
        {
            if (string.IsNullOrEmpty(url)) return;
            var path = Path.Combine(_env.WebRootPath, url.TrimStart('/'));
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.Contract;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize]
  
    public class ContractsController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IWebHostEnvironment _env;

        public ContractsController(IContractService contractService, IWebHostEnvironment env)
        {
            _contractService = contractService;
            _env = env;
        }

        [PermissionAuthorize("Contracts.View")]
        public IActionResult Index()
        {
            var result = _contractService.GetAll();
            return View(result.Data);
        }

        [HttpGet]
        [PermissionAuthorize("Contracts.View")]
        public IActionResult Create() => View(new ContractCreateDto());

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Contracts.Manage")]
        public IActionResult Create(ContractCreateDto dto, IFormFile? PdfFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return View(dto);
            }
            if (PdfFile != null)
                dto.PdfUrl = SavePdf(PdfFile);

            var result = _contractService.Add(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        [PermissionAuthorize("Contracts.Manage")]
        public IActionResult Edit(int id)
        {
            var result = _contractService.GetById(id);
            if (!result.Success) return NotFound();

            var dto = new ContractUpdateDto
            {
                ContractId = result.Data.ContractId,
                Title = result.Data.Title,
                Description = result.Data.Description,
                ContentText = result.Data.ContentText,
                IsActive = result.Data.IsActive,
                ExistingPdfUrl = result.Data.PdfUrl
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Contracts.Manage")]
        public IActionResult Edit(ContractUpdateDto dto, IFormFile? PdfFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları doldurun.";
                return View(dto);
            }
            if (PdfFile != null)
            {
                DeletePdf(dto.ExistingPdfUrl);
                dto.PdfUrl = SavePdf(PdfFile);
            }
            var result = _contractService.Update(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Contracts.Delete")]
        public IActionResult Delete(int id)
        {
            var contract = _contractService.GetById(id);
            if (contract.Success) DeletePdf(contract.Data.PdfUrl);
            var result = _contractService.Delete(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [PermissionAuthorize("Contracts.Manage")]
        public IActionResult ToggleStatus(int id)
        {
            var result = _contractService.GetById(id);
            if (!result.Success) return NotFound();
            var dto = new ContractUpdateDto
            {
                ContractId = result.Data.ContractId,
                Title = result.Data.Title,
                Description = result.Data.Description,
                ContentText = result.Data.ContentText,
                IsActive = !result.Data.IsActive,
                ExistingPdfUrl = result.Data.PdfUrl
            };
            _contractService.Update(dto);
            return Ok();
        }

        private string SavePdf(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "files", "contracts");
            Directory.CreateDirectory(folder);
            var fileName = Guid.NewGuid() + ".pdf";
            var path = Path.Combine(folder, fileName);
            using var stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            return "/files/contracts/" + fileName;
        }

        private void DeletePdf(string? url)
        {
            if (string.IsNullOrEmpty(url)) return;
            var path = Path.Combine(_env.WebRootPath, url.TrimStart('/'));
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
    }
}
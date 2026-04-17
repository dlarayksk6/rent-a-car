using RentACar.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.DTOs.Blog;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Personel")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _env;

        public BlogController(IBlogService blogService, IWebHostEnvironment env)
        {
            _blogService = blogService;
            _env = env;
        }

        [PermissionAuthorize("Blog.View")]
        public IActionResult Index()
        {
            var result = _blogService.GetAll();
            return View(result.Data);
        }

        [PermissionAuthorize("Blog.View")]
        public IActionResult Details(int id)
        {
            var result = _blogService.GetById(id);
            if (!result.Success) return NotFound();
            return View(result.Data);
        }

        [HttpGet]
        [PermissionAuthorize("Blog.Manage")]
        public IActionResult Create()
        {
            return View(new BlogCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Blog.Manage")]
        public async Task<IActionResult> Create(BlogCreateDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View(dto);
            }

            dto.AuthorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(dto.AuthorId))
            {
                TempData["Error"] = "Kullanıcı kimliği alınamadı!";
                return View(dto);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "blog");
                Directory.CreateDirectory(folder);
                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
                await imageFile.CopyToAsync(stream);
                dto.ImageUrl = "/uploads/blog/" + fileName;
            }

            var result = _blogService.Add(dto, _env.WebRootPath);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            if (result.Success)
                return RedirectToAction(nameof(Index));

            return View(dto);
        }

        [HttpGet]
        [PermissionAuthorize("Blog.Manage")]
        public IActionResult Edit(int id)
        {
            var result = _blogService.GetById(id);
            if (!result.Success) return NotFound();

            var dto = new BlogUpdateDto
            {
                BlogPostId = result.Data.BlogPostId,
                Title = result.Data.Title,
                Content = result.Data.Content,
                IsApproved = result.Data.IsApproved,
                ExistingImageUrl = result.Data.ImageUrl
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Blog.Manage")]
        public async Task<IActionResult> Edit(BlogUpdateDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View(dto);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "blog");
                Directory.CreateDirectory(folder);
                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
                await imageFile.CopyToAsync(stream);
                dto.ImageUrl = "/uploads/blog/" + fileName;
            }

            var result = _blogService.Update(dto, _env.WebRootPath);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            if (result.Success)
                return RedirectToAction(nameof(Index));

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Blog.Approve")]
        public IActionResult Approve(int id)
        {
            var result = _blogService.Approve(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Blog.Delete")]
        public IActionResult Delete(int id)
        {
            var result = _blogService.Delete(id, _env.WebRootPath);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
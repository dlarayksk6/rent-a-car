using RentACar.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.DTOs.User;
using RentACar.WebUI.Attributes;
using RentACar.Entities;

namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Personel")]
    [PermissionAuthorize("Users.Manage")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public UsersController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllAsync();
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            var permissions = _permissionService.GetAll();
            ViewBag.Permissions = permissions.Data ?? new List<Permission>();
            return View(new UserCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            { 
                var permissions = _permissionService.GetAll();
                ViewBag.Permissions = permissions.Data ?? new List<Permission>();
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View(dto);
            }
            var result = await _userService.CreateAsync(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            if (result.Success)
                return RedirectToAction("Index");

            var perms = _permissionService.GetAll();
            ViewBag.Permissions = perms.Data ?? new List<Permission>();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var allUsers = await _userService.GetAllAsync();
            var user = allUsers.Data?.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Index");
            }
            ViewBag.User = user;

            var permissions = _permissionService.GetAll();
            ViewBag.Permissions = permissions.Data ?? new List<Permission>();

            var userPerms = await _permissionService.GetUserPermissionsAsync(id);
            ViewBag.UserPermissions = userPerms ?? new List<int>();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, List<int> selectedPermissions)
        {
            var result = await _userService.UpdatePermissionsAsync(id, selectedPermissions);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
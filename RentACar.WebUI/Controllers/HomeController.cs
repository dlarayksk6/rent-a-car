using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;


namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Personel")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            var result = _dashboardService.GetDashboardData();
            return View(result.Data);
        }


        public IActionResult Error(int? statusCode)
        {
            ViewBag.StatusCode = statusCode ?? 500;
            ViewBag.Message = statusCode switch
            {
                404 => "Aradığınız sayfa bulunamadı.",
                403 => "Bu sayfaya erişim izniniz yok.",
                500 => "Sunucuda bir hata oluştu.",
                _ => "Beklenmeyen bir hata oluştu."
            };
            return View();
        }
    }
}
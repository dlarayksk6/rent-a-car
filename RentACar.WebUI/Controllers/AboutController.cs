using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.About;
using RentACar.Entities;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Personel")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [PermissionAuthorize("About.Edit")]
        public IActionResult Index()
        {
            var result = _aboutService.Get();
            if (result.Success)
            {
                var dto = new AboutUpdateDto
                {
                    AboutContentId = result.Data.AboutContentId,
                    Title = result.Data.Title,
                    SubTitle = result.Data.SubTitle,
                    Description1 = result.Data.Description1,
                    Description2 = result.Data.Description2,
                    Description3 = result.Data.Description3,
                    Feature1Title = result.Data.Feature1Title,
                    Feature1Text = result.Data.Feature1Text,
                    Feature2Title = result.Data.Feature2Title,
                    Feature2Text = result.Data.Feature2Text,
                    Feature3Title = result.Data.Feature3Title,
                    Feature3Text = result.Data.Feature3Text,
                    Feature4Title = result.Data.Feature4Title,
                    Feature4Text = result.Data.Feature4Text,
                    BannerTitle = result.Data.BannerTitle,
                    BannerText = result.Data.BannerText,
                    CompletedOrders = result.Data.CompletedOrders,
                    HappyCustomers = result.Data.HappyCustomers,
                    CarFleet = result.Data.CarFleet,
                    YearsExperience = result.Data.YearsExperience,
                    ExistingImageUrl = result.Data.ImageUrl,
                    HeroTitle = result.Data.HeroTitle,
                    HeroSubText = result.Data.HeroSubText,
                    HeroFeature1Title = result.Data.HeroFeature1Title,
                    HeroFeature1Text = result.Data.HeroFeature1Text,
                    HeroFeature2Title = result.Data.HeroFeature2Title,
                    HeroFeature2Text = result.Data.HeroFeature2Text
                };
                return View(dto);
            }
            return View(new AboutUpdateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("About.Edit")]
        public IActionResult Update(AboutUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View("Index", dto);
            }
            var result = _aboutService.Update(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
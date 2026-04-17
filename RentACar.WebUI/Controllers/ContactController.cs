using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.DTOs.Contact;
using RentACar.WebUI.Attributes;

namespace RentACar.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [PermissionAuthorize("Contact.Edit")]

    public class ContactController : Controller
    {
        private readonly ISiteContactService _siteContactService;

        public ContactController(ISiteContactService siteContactService)
        {
            _siteContactService = siteContactService;
        }

        public IActionResult Index()
        {
            var result = _siteContactService.Get();
            if (result.Success)
            {
                var dto = new ContactUpdateDto
                {
                    SiteContactId = result.Data.SiteContactId,
                    Phone = result.Data.Phone,
                    Email = result.Data.Email,
                    Address = result.Data.Address,
                    Facebook = result.Data.Facebook,
                    Instagram = result.Data.Instagram,
                    Twitter = result.Data.Twitter,
                    WhatsappNumber = result.Data.WhatsappNumber,
                    WorkingHours = result.Data.WorkingHours
                };
                return View(dto);
            }
            return View(new ContactUpdateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateContact(ContactUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen zorunlu alanları eksiksiz doldurun.";
                return View("Index", dto);
            }
            var result = _siteContactService.Update(dto);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
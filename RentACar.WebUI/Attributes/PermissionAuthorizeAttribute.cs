using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Entities;

using System.Security.Claims;
using RentACar.Business.Abstract;
namespace RentACar.WebUI.Attributes
{
    public class PermissionAuthorizeAttribute : TypeFilterAttribute
    {
        public PermissionAuthorizeAttribute(string permission) : base(typeof(PermissionAuthorizeFilter))
        {
            Arguments = new object[] { permission };
        }
    }
    public class PermissionAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permission;
        private readonly IPermissionService _permissionService;

        public PermissionAuthorizeFilter(string permission, IPermissionService permissionService)
        {
            _permission = permission;
            _permissionService = permissionService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "Identity" });
                return;
            }

           
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new ContentResult
                {
                    Content = "Kullanıcı ID alınamadı.",
                    StatusCode = 401
                };
                return;
            }


            var hasPermission = await _permissionService.HasPermissionAsync(userId, _permission);
            if (!hasPermission)
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { statusCode = 403 });
            }
        }

    }

}

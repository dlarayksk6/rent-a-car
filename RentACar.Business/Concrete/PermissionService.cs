using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Business.Concrete
{
    public class PermissionService : IPermissionService
    {
        private readonly RentACarDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionService(RentACarDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> HasPermissionAsync(string permissionCode)
        {
            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return false;
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin")) return true;

            return await _context.UserPermissions
                .Include(up => up.Permission)
                .AnyAsync(up => up.UserId == userId && up.Permission.Code == permissionCode);
        }

        public async Task<bool> HasPermissionAsync(string userId, string permissionCode)
        {
            if (string.IsNullOrEmpty(userId)) return false;
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin")) return true;

            return await _context.UserPermissions
                .Include(up => up.Permission)
                .AnyAsync(up => up.UserId == userId && up.Permission.Code == permissionCode);
        }
        public IDataResult<List<Permission>> GetAll()
        {
            var permissions = _context.Permissions.OrderBy(p => p.Name).ToList();
            return new SuccessDataResult<List<Permission>>(permissions);
        }

        public async Task<List<string>> GetCurrentUserPermissionsAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return new List<string>();

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
                return await _context.Permissions.Select(p => p.Code).ToListAsync();

            return await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Include(up => up.Permission)
                .Select(up => up.Permission.Code)
                .ToListAsync();
        }
        public async Task<List<int>> GetUserPermissionsAsync(string userId)
        {
            return await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.PermissionId)
                .ToListAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IResult = RentACar.Core.Results.IResult;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.User;
using RentACar.Entities;

namespace RentACar.Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RentACarDbContext _context;

        public UserService(UserManager<IdentityUser> userManager, RentACarDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IDataResult<List<UserListDto>>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserListDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var permissions = await _context.UserPermissions
                    .Where(up => up.UserId == user.Id)
                    .Include(up => up.Permission)
                    .Select(up => up.Permission.Name)
                    .ToListAsync();

                result.Add(new UserListDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Permissions = permissions
                });
            }

            return new SuccessDataResult<List<UserListDto>>(result);
        }

        public async Task<IResult> CreateAsync(UserCreateDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                return new ErrorResult("Email ve şifre zorunludur.");

            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return new ErrorResult(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Personel");

            if (dto.SelectedPermissions?.Count > 0)
            {
                foreach (var permissionId in dto.SelectedPermissions)
                    _context.UserPermissions.Add(new UserPermission { UserId = user.Id, PermissionId = permissionId });

                await _context.SaveChangesAsync();
            }

            return new SuccessResult("Personel oluşturuldu.");
        }

        public async Task<IResult> UpdatePermissionsAsync(string userId, List<int> selectedPermissions)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            var existing = _context.UserPermissions.Where(up => up.UserId == userId);
            _context.UserPermissions.RemoveRange(existing);

            if (selectedPermissions?.Count > 0)
                foreach (var permId in selectedPermissions)
                    _context.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = permId });

            await _context.SaveChangesAsync();
            return new SuccessResult("Yetkiler güncellendi.");
        }

        public async Task<IResult> DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin")) return new ErrorResult("Admin silinemez.");

            var perms = _context.UserPermissions.Where(up => up.UserId == userId);
            _context.UserPermissions.RemoveRange(perms);
            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return new ErrorResult("Silme işlemi başarısız.");

            return new SuccessResult("Personel silindi.");
        }
    }
}
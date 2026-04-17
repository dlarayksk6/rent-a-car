using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RentACar.Core.Results;
using RentACar.DTOs.User;
using IResult = RentACar.Core.Results.IResult;
namespace RentACar.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<UserListDto>>> GetAllAsync();
        Task<IResult> CreateAsync(UserCreateDto dto);
        Task<IResult> UpdatePermissionsAsync(string userId, List<int> selectedPermissions);
        Task<IResult> DeleteAsync(string userId);
    }
}
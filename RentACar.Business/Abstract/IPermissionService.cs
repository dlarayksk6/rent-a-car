using RentACar.Core.Results;
using RentACar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Business.Abstract
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string permissionCode);
        Task<bool> HasPermissionAsync(string userId, string permissionCode);
        IDataResult<List<Permission>> GetAll();
        Task<List<string>> GetCurrentUserPermissionsAsync();
        Task<List<int>> GetUserPermissionsAsync(string userId);
    }
}
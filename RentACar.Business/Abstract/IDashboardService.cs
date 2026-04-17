using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RentACar.Core.Results;
using RentACar.DTOs.Dashboard;

namespace RentACar.Business.Abstract
{
    public interface IDashboardService
    {
        IDataResult<DashboardDto> GetDashboardData();
    }
}
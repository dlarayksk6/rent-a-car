using RentACar.Core.Results;
using RentACar.DTOs.Brand;
using RentACar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Business.Abstract
{
    public interface IBrandService
    {
        IDataResult<List<Brand>> GetAll();
        IDataResult<List<Brand>> GetActive();
        IDataResult<Brand> GetById(int id);
        IResult Add(BrandCreateDto brand);
        IResult Update(BrandUpdateDto brand);
        IResult Delete(int id);
        IResult ToggleStatus(int id);
    }
}

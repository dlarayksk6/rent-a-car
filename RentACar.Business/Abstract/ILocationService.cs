using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ILocationService.cs
using RentACar.Core.Results;
using RentACar.DTOs.Location;
using RentACar.Entities;

namespace RentACar.Business.Abstract
{
    public interface ILocationService
    {
        IDataResult<List<Location>> GetAll();
        IDataResult<List<Location>> GetPickupLocations();
        IDataResult<List<Location>> GetDropoffLocations();
        IDataResult<Location> GetById(int id);
        IResult Add(LocationCreateDto dto);
        IResult Update(LocationUpdateDto dto);
        IResult Delete(int id);
    }
}

using RentACar.Core.Results;
using RentACar.DTOs.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IResult = RentACar.Core.Results.IResult;

namespace RentACar.Business.Abstract
{
    public interface ICarService
    {
        IDataResult<List<CarListDto>> GetAll();
        IDataResult<List<CarListDto>> GetAllActive();
        IDataResult<CarListDto> GetById(int carId);
       
        IResult Add(CarCreateDto dto, string webRootPath);
        IResult Update(CarUpdateDto dto, string webRootPath);

        IResult Delete(int carId, string webRootPath);
        IDataResult<List<CarListDto>> GetFiltered(
     string[] category, string[] fuel, string[] transmission,
     string[] seats, int? minYear, int? maxYear,
     decimal? minPrice, decimal? maxPrice,
     int? maxMinAge, int? maxMinLicense,
     string[] colors, string[] brands,
     string[] bodyTypes, int? minLuggage,
     int? minHorsePower,
     bool? hasAC, bool? hasBluetooth,
     bool? hasNavigation, bool? hasBackCamera,
     bool? hasSunroof, bool? hasHeatedSeats,
     bool? hasCruiseControl, bool? hasParkingSensor,
     bool? hasChildSeat, bool? hasUSBPort);
    }
}
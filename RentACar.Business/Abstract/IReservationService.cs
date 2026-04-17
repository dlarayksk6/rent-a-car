using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RentACar.Core.Results;
using RentACar.DTOs.Reservation;
using IResult = RentACar.Core.Results.IResult;
namespace RentACar.Business.Abstract
{
    public interface IReservationService
    {
        IDataResult<List<ReservationListDto>> GetAll(string search = "", string status = "");
        IDataResult<ReservationListDto> GetById(int reservationId);
        IDataResult<List<ReservationListDto>> GetByCarId(int carId);
        IResult Add(ReservationCreateDto dto);
        IResult Update(ReservationUpdateDto dto);

        IResult Cancel(int reservationId);
        IResult Delete(int reservationId);

        IResult BlockDates(int carId, DateTime startDate, DateTime endDate, string blockType = "Kiralama", string notes = "", decimal totalPrice = 0);

    }
}
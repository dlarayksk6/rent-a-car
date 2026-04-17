using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RentACar.Core.Results;
using RentACar.DTOs.Contact;
using RentACar.Entities;
using IResult = RentACar.Core.Results.IResult;
namespace RentACar.Business.Abstract
{
    public interface ISiteContactService
    {
        IDataResult<SiteContact> Get();
        IResult Update(ContactUpdateDto dto);
    }
}
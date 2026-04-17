using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RentACar.Core.Results;
using RentACar.Entities;
using IResult = RentACar.Core.Results.IResult;

using RentACar.Core.Results;
using RentACar.DTOs.About;
using RentACar.Entities;

namespace RentACar.Business.Abstract
{
    public interface IAboutService
    {
        IDataResult<AboutContent> Get();
        IResult Update(AboutUpdateDto dto);
    }
}
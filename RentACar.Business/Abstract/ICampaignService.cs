using RentACar.Core.Results;
using RentACar.DTOs.Campaign;
using RentACar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Business.Abstract
{
    public interface ICampaignService
    {
        IDataResult<List<Campaign>> GetAll();
        IDataResult<List<Campaign>> GetActive();
        IDataResult<Campaign> GetById(int id);
        IResult Add(CampaignCreateDto dto);
        IResult Update(CampaignUpdateDto dto);
        IResult Delete(int id);
    }
}

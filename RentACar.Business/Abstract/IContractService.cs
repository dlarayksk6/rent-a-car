using RentACar.Core.Results;
using RentACar.DTOs.Contract;
using RentACar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Business.Abstract
{
    public interface IContractService
    {
        IDataResult<List<ContractDocument>> GetAll();
        IDataResult<List<ContractDocument>> GetActive();
        IDataResult<ContractDocument> GetById(int id);
        IResult Add(ContractCreateDto dto);
        IResult Update(ContractUpdateDto dto);
        IResult Delete(int id);
    }
}

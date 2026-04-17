using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Contract;
using RentACar.Entities;

namespace RentACar.Business.Concrete
{
    public class ContractService : IContractService
    {
        private readonly RentACarDbContext _context;

        public ContractService(RentACarDbContext context) => _context = context;

        public IDataResult<List<ContractDocument>> GetAll() =>
            new SuccessDataResult<List<ContractDocument>>(_context.ContractDocuments.ToList());

        public IDataResult<List<ContractDocument>> GetActive() =>
            new SuccessDataResult<List<ContractDocument>>(_context.ContractDocuments.Where(c => c.IsActive).ToList());

        public IDataResult<ContractDocument> GetById(int id)
        {
            var c = _context.ContractDocuments.Find(id);
            return c == null ? new ErrorDataResult<ContractDocument>("Bulunamadı.") : new SuccessDataResult<ContractDocument>(c);
        }

        public IResult Add(ContractCreateDto dto)
        {
            var contract = new ContractDocument
            {
                Title = dto.Title,
                Description = dto.Description,
                ContentText = dto.ContentText,
                PdfUrl = dto.PdfUrl,
                IsActive = dto.IsActive
            };
            _context.ContractDocuments.Add(contract);
            _context.SaveChanges();
            return new SuccessResult("Sözleşme eklendi.");
        }

        public IResult Update(ContractUpdateDto dto)
        {
            var contract = _context.ContractDocuments.Find(dto.ContractId);
            if (contract == null) return new ErrorResult("Sözleşme bulunamadı.");

            contract.Title = dto.Title;
            contract.Description = dto.Description;
            contract.ContentText = dto.ContentText;
            contract.IsActive = dto.IsActive;
            if (!string.IsNullOrEmpty(dto.PdfUrl))
                contract.PdfUrl = dto.PdfUrl;

            _context.ContractDocuments.Update(contract);
            _context.SaveChanges();
            return new SuccessResult("Sözleşme güncellendi.");
        }

        public IResult Delete(int id)
        {
            var c = _context.ContractDocuments.Find(id);
            if (c == null) return new ErrorResult("Bulunamadı.");
            _context.ContractDocuments.Remove(c);
            _context.SaveChanges();
            return new SuccessResult("Sözleşme silindi.");
        }
    }
}
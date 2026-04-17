using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Campaign;
using RentACar.Entities;

namespace RentACar.Business.Concrete
{
    public class CampaignService : ICampaignService
    {
        private readonly RentACarDbContext _context;

        public CampaignService(RentACarDbContext context) => _context = context;

        public IDataResult<List<Campaign>> GetAll() =>
            new SuccessDataResult<List<Campaign>>(_context.Campaigns.OrderByDescending(c => c.CreatedAt).ToList());

        public IDataResult<List<Campaign>> GetActive() =>
            new SuccessDataResult<List<Campaign>>(_context.Campaigns.Where(c => c.IsActive).OrderByDescending(c => c.CreatedAt).ToList());

        public IDataResult<Campaign> GetById(int id)
        {
            var c = _context.Campaigns.Find(id);
            return c == null ? new ErrorDataResult<Campaign>("Bulunamadı.") : new SuccessDataResult<Campaign>(c);
        }

        public IResult Add(CampaignCreateDto dto)
        {
            var campaign = new Campaign
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();
            return new SuccessResult("Kampanya eklendi.");
        }

        public IResult Update(CampaignUpdateDto dto)
        {
            var campaign = _context.Campaigns.Find(dto.CampaignId);
            if (campaign == null) return new ErrorResult("Kampanya bulunamadı.");

            campaign.Title = dto.Title;
            campaign.Description = dto.Description;
            campaign.IsActive = dto.IsActive;
            if (!string.IsNullOrEmpty(dto.ImageUrl))
                campaign.ImageUrl = dto.ImageUrl;

            _context.Campaigns.Update(campaign);
            _context.SaveChanges();
            return new SuccessResult("Kampanya güncellendi.");
        }

        public IResult Delete(int id)
        {
            var c = _context.Campaigns.Find(id);
            if (c == null) return new ErrorResult("Bulunamadı.");
            _context.Campaigns.Remove(c);
            _context.SaveChanges();
            return new SuccessResult("Kampanya silindi.");
        }
    }
}
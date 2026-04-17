using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.About;
using RentACar.Entities;
using IResult = RentACar.Core.Results.IResult;

namespace RentACar.Business.Concrete
{
    public class AboutService : IAboutService
    {
        private readonly RentACarDbContext _context;

        public AboutService(RentACarDbContext context)
        {
            _context = context;
        }

        public IDataResult<AboutContent> Get()
        {
            var about = _context.AboutContents.FirstOrDefault();
            if (about == null)
                return new ErrorDataResult<AboutContent>("İçerik bulunamadı.");
            return new SuccessDataResult<AboutContent>(about);
        }

        public IResult Update(AboutUpdateDto dto)
        {
            var about = _context.AboutContents.FirstOrDefault();
            if (about == null)
            {
                var newAbout = new AboutContent
                {
                    Title = dto.Title,
                    SubTitle = dto.SubTitle,
                    Description1 = dto.Description1,
                    Description2 = dto.Description2,
                    Description3 = dto.Description3,
                    Feature1Title = dto.Feature1Title,
                    Feature1Text = dto.Feature1Text,
                    Feature2Title = dto.Feature2Title,
                    Feature2Text = dto.Feature2Text,
                    Feature3Title = dto.Feature3Title,
                    Feature3Text = dto.Feature3Text,
                    Feature4Title = dto.Feature4Title,
                    Feature4Text = dto.Feature4Text,
                    BannerTitle = dto.BannerTitle,
                    BannerText = dto.BannerText,
                    CompletedOrders = dto.CompletedOrders,
                    HappyCustomers = dto.HappyCustomers,
                    CarFleet = dto.CarFleet,
                    YearsExperience = dto.YearsExperience,
                    ImageUrl = dto.ImageUrl,
                    HeroTitle = dto.HeroTitle,
                    HeroSubText = dto.HeroSubText,
                    HeroFeature1Title = dto.HeroFeature1Title,
                    HeroFeature1Text = dto.HeroFeature1Text,
                    HeroFeature2Title = dto.HeroFeature2Title,
                    HeroFeature2Text = dto.HeroFeature2Text,
                    UpdatedAt = DateTime.Now
                };
                _context.AboutContents.Add(newAbout);
            }
            else
            {
                about.Title = dto.Title;
                about.SubTitle = dto.SubTitle;
                about.Description1 = dto.Description1;
                about.Description2 = dto.Description2;
                about.Description3 = dto.Description3;
                about.Feature1Title = dto.Feature1Title;
                about.Feature1Text = dto.Feature1Text;
                about.Feature2Title = dto.Feature2Title;
                about.Feature2Text = dto.Feature2Text;
                about.Feature3Title = dto.Feature3Title;
                about.Feature3Text = dto.Feature3Text;
                about.Feature4Title = dto.Feature4Title;
                about.Feature4Text = dto.Feature4Text;
                about.BannerTitle = dto.BannerTitle;
                about.BannerText = dto.BannerText;
                about.CompletedOrders = dto.CompletedOrders;
                about.HappyCustomers = dto.HappyCustomers;
                about.CarFleet = dto.CarFleet;
                about.YearsExperience = dto.YearsExperience;
                if (!string.IsNullOrEmpty(dto.ImageUrl))
                    about.ImageUrl = dto.ImageUrl;
                about.HeroTitle = dto.HeroTitle;
                about.HeroSubText = dto.HeroSubText;
                about.HeroFeature1Title = dto.HeroFeature1Title;
                about.HeroFeature1Text = dto.HeroFeature1Text;
                about.HeroFeature2Title = dto.HeroFeature2Title;
                about.HeroFeature2Text = dto.HeroFeature2Text;
                about.UpdatedAt = DateTime.Now;
                _context.AboutContents.Update(about);
            }
            _context.SaveChanges();
            return new SuccessResult("Hakkımızda içeriği güncellendi.");
        }
    }
}
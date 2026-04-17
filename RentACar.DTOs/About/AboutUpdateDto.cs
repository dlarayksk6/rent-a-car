using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DTOs.About
{
    public class AboutUpdateDto
    {
        public int AboutContentId { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description3 { get; set; }
        public string? Feature1Title { get; set; }
        public string? Feature1Text { get; set; }
        public string? Feature2Title { get; set; }
        public string? Feature2Text { get; set; }
        public string? Feature3Title { get; set; }
        public string? Feature3Text { get; set; }
        public string? Feature4Title { get; set; }
        public string? Feature4Text { get; set; }
        public string? BannerTitle { get; set; }
        public string? BannerText { get; set; }
        public int? CompletedOrders { get; set; }
        public int? HappyCustomers { get; set; }
        public int? CarFleet { get; set; }
        public int? YearsExperience { get; set; }
        public string? ImageUrl { get; set; }
        public string? ExistingImageUrl { get; set; }
        public string? HeroTitle { get; set; }
        public string? HeroSubText { get; set; }
        public string? HeroFeature1Title { get; set; }
        public string? HeroFeature1Text { get; set; }
        public string? HeroFeature2Title { get; set; }
        public string? HeroFeature2Text { get; set; }
    }
}
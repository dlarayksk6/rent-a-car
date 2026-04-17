using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using RentACar.DTOs.About;

namespace RentACar.Business.Validators.About
{
    public class AboutUpdateValidator : AbstractValidator<AboutUpdateDto>
    {
        public AboutUpdateValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.")
                .When(x => x.Title != null);

            RuleFor(x => x.SubTitle)
                .MaximumLength(300).WithMessage("Alt başlık en fazla 300 karakter olabilir.")
                .When(x => x.SubTitle != null);

            RuleFor(x => x.HeroTitle)
                .MaximumLength(200).WithMessage("Hero başlık en fazla 200 karakter olabilir.")
                .When(x => x.HeroTitle != null);

            RuleFor(x => x.BannerTitle)
                .MaximumLength(300).WithMessage("Banner başlık en fazla 300 karakter olabilir.")
                .When(x => x.BannerTitle != null);

            RuleFor(x => x.CompletedOrders)
                .GreaterThanOrEqualTo(0).WithMessage("Tamamlanan sipariş 0 veya daha büyük olmalıdır.")
                .When(x => x.CompletedOrders.HasValue);

            RuleFor(x => x.HappyCustomers)
                .GreaterThanOrEqualTo(0).WithMessage("Mutlu müşteri sayısı 0 veya daha büyük olmalıdır.")
                .When(x => x.HappyCustomers.HasValue);

            RuleFor(x => x.CarFleet)
                .GreaterThanOrEqualTo(0).WithMessage("Araç filosu 0 veya daha büyük olmalıdır.")
                .When(x => x.CarFleet.HasValue);

            RuleFor(x => x.YearsExperience)
                .GreaterThanOrEqualTo(0).WithMessage("Yıllık deneyim 0 veya daha büyük olmalıdır.")
                .When(x => x.YearsExperience.HasValue);
        }
    }
}

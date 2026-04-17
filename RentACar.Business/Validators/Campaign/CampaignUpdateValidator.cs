using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RentACar.DTOs.Campaign;

namespace RentACar.Business.Validators.Campaign
{
    public class CampaignUpdateValidator : AbstractValidator<CampaignUpdateDto>
    {
        public CampaignUpdateValidator()
        {
            RuleFor(x => x.CampaignId)
                .GreaterThan(0).WithMessage("Geçerli bir kampanya seçilmelidir.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kampanya başlığı zorunludur.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.")
                .When(x => x.Description != null);
        }
    }
}

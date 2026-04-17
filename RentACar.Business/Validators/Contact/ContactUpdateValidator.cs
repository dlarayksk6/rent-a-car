using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using RentACar.DTOs.Contact;

namespace RentACar.Business.Validators.SiteContact
{
    public class SiteContactUpdateValidator : AbstractValidator<ContactUpdateDto>
    {
        public SiteContactUpdateValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon zorunludur.")
                .MaximumLength(20).WithMessage("Telefon en fazla 20 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin.")
                .MaximumLength(150).WithMessage("E-posta en fazla 150 karakter olabilir.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres zorunludur.")
                .MaximumLength(300).WithMessage("Adres en fazla 300 karakter olabilir.");

            RuleFor(x => x.WhatsappNumber)
                .MaximumLength(20).WithMessage("WhatsApp numarası en fazla 20 karakter olabilir.")
                .When(x => x.WhatsappNumber != null);

            RuleFor(x => x.WorkingHours)
                .MaximumLength(100).WithMessage("Çalışma saatleri en fazla 100 karakter olabilir.")
                .When(x => x.WorkingHours != null);
        }
    }
}
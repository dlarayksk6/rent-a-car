using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using RentACar.DTOs.Brand;

namespace RentACar.Business.Validators.Brand
{
    public class BrandUpdateValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateValidator()
        {
            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Geçerli bir marka seçilmelidir.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Marka adı zorunludur.")
                .MaximumLength(100).WithMessage("Marka adı en fazla 100 karakter olabilir.");
        }
    }
}
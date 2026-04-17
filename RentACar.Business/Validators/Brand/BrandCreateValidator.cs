using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using RentACar.DTOs.Brand;

namespace RentACar.Business.Validators.Brand
{
    public class BrandCreateValidator : AbstractValidator<BrandCreateDto>
    {
        public BrandCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Marka adı zorunludur.")
                .MaximumLength(100).WithMessage("Marka adı en fazla 100 karakter olabilir.");
        }
    }
}
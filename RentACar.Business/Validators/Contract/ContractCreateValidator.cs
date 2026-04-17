using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using RentACar.DTOs.Contract;

namespace RentACar.Business.Validators.Contract
{
    public class ContractCreateValidator : AbstractValidator<ContractCreateDto>
    {
        public ContractCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Sözleşme başlığı zorunludur.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.")
                .When(x => x.Description != null);
        }
    }
}
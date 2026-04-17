using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RentACar.DTOs.Car;

namespace RentACar.Business.Validators.Car
{
    public class CarCreateValidator : AbstractValidator<CarCreateDto>
    {
        public CarCreateValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Marka zorunludur.")
                .MaximumLength(100).WithMessage("Marka en fazla 100 karakter olabilir.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model zorunludur.")
                .MaximumLength(100).WithMessage("Model en fazla 100 karakter olabilir.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1990, 2030).WithMessage("Yıl 1990-2030 arasında olmalıdır.");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Renk zorunludur.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Kategori zorunludur.");

            RuleFor(x => x.PlateNumber)
                .NotEmpty().WithMessage("Plaka zorunludur.")
                .MaximumLength(20).WithMessage("Plaka en fazla 20 karakter olabilir.");

            RuleFor(x => x.DailyPrice)
                .GreaterThan(0).WithMessage("Günlük fiyat 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(100000).WithMessage("Fiyat en fazla 100.000 olabilir.");

            RuleFor(x => x.Transmission)
                .NotEmpty().WithMessage("Vites tipi zorunludur.");

            RuleFor(x => x.FuelType)
                .NotEmpty().WithMessage("Yakıt tipi zorunludur.");

            RuleFor(x => x.Seats)
                .InclusiveBetween(1, 20).WithMessage("Koltuk sayısı 1-20 arasında olmalıdır.");

            RuleFor(x => x.MinAge)
                .InclusiveBetween(18, 99).WithMessage("Minimum yaş 18-99 arasında olmalıdır.");

            RuleFor(x => x.MinDriverLicenseYear)
                .InclusiveBetween(0, 50).WithMessage("Ehliyet yılı 0-50 arasında olmalıdır.");

            RuleFor(x => x.DepositAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Depozito 0 veya daha büyük olmalıdır.")
                .When(x => x.DepositAmount.HasValue);

            RuleFor(x => x.ExtraKmPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Ekstra km ücreti 0 veya daha büyük olmalıdır.")
                .When(x => x.ExtraKmPrice.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir.")
                .When(x => x.Description != null);
        }
    }
}
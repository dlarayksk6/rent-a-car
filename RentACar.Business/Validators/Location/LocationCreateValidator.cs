using FluentValidation;
using RentACar.DTOs.Location;

namespace RentACar.Business.Validators.Location
{
    public class LocationCreateValidator : AbstractValidator<LocationCreateDto>
    {
        public LocationCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Lokasyon adı zorunludur.")
                .MaximumLength(200).WithMessage("Lokasyon adı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Address)
                .MaximumLength(300).WithMessage("Adres en fazla 300 karakter olabilir.")
                .When(x => x.Address != null);
        }
    }
}
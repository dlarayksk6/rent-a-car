using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using RentACar.DTOs.Reservation;

namespace RentACar.Business.Validators.Reservation
{
    public class ReservationCreateValidator : AbstractValidator<ReservationCreateDto>
    {
        public ReservationCreateValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("Araç seçimi zorunludur.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Başlangıç tarihi zorunludur.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Başlangıç tarihi geçmiş olamaz.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Bitiş tarihi zorunludur.")
                .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

            RuleFor(x => x.PickupLocation)
                .NotEmpty().WithMessage("Alış yeri zorunludur.")
                .MaximumLength(200).WithMessage("Alış yeri en fazla 200 karakter olabilir.");

            RuleFor(x => x.DropoffLocation)
                .NotEmpty().WithMessage("Bırakış yeri zorunludur.")
                .MaximumLength(200).WithMessage("Bırakış yeri en fazla 200 karakter olabilir.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad Soyad zorunludur.")
                .MaximumLength(150).WithMessage("Ad Soyad en fazla 150 karakter olabilir.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası zorunludur.")
                .MaximumLength(20).WithMessage("Telefon en fazla 20 karakter olabilir.");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("TC Kimlik No zorunludur.")
                .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.")
                .Matches("^[0-9]+$").WithMessage("TC Kimlik No sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notlar en fazla 500 karakter olabilir.")
                .When(x => x.Notes != null);
        }
    }
}
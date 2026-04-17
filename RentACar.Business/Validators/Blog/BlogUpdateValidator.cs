using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using RentACar.DTOs.Blog;

namespace RentACar.Business.Validators.Blog
{
    public class BlogUpdateValidator : AbstractValidator<BlogUpdateDto>
    {
        public BlogUpdateValidator()
        {
            RuleFor(x => x.BlogPostId)
                .GreaterThan(0).WithMessage("Geçerli bir blog yazısı seçilmelidir.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık zorunludur.")
                .MaximumLength(300).WithMessage("Başlık en fazla 300 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik zorunludur.")
                .MinimumLength(50).WithMessage("İçerik en az 50 karakter olmalıdır.");
        }
    }
}
using DTO;
using DTO.AdRequest;
using FluentValidation;

namespace Infrastructure.Services.Validations
{
    public class AdValidator : AbstractValidator<AdvertisementRequest>
    {
        public AdValidator()
        {
            RuleFor(i => i.Title)
                .NotEmpty().WithMessage("Title cannot be empty")
                .MaximumLength(20).WithMessage("Title can include maximum 20 symbols");

            RuleFor(i => i.Text)
                .NotEmpty().WithMessage("Text cannot be empty");
        }
    }
}
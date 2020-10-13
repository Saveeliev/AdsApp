using DTO;
using FluentValidation;

namespace Infrastructure.Services.Validations
{
    public class AdValidator : AbstractValidator<AdDto>
    {
        public AdValidator()
        {
            RuleFor(i => i.Text).NotEmpty().WithMessage("Text cannot be empty");
        }
    }
}
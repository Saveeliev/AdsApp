using AdsApp.Models.ViewModels;
using FluentValidation;

namespace AdsApp.Validations
{
    public class AdValidator : AbstractValidator<AdDto>
    {
        public AdValidator()
        {
            RuleFor(i => i.Text).NotEmpty().WithMessage("Text cannot be empty");
        }
    }
}
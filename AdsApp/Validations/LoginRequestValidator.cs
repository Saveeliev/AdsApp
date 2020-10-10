using AdsApp.Models.DTO;
using AdsApp.Models.ViewModels;
using FluentValidation;

namespace AdsApp.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(i => i.Login)
                .NotEmpty().WithMessage("Login cannot be empty");

            RuleFor(i => i.Password)
                .NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
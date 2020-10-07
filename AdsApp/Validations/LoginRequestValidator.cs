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
                .NotNull().WithMessage("Login cannot be empty");

            RuleFor(i => i.Password)
                .NotNull().WithMessage("Password cannot be empty");
        }
    }
}
using DTO.Request;
using FluentValidation;

namespace Infrastructure.Services.Validations
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
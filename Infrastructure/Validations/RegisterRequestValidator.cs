using DTO.Request;
using FluentValidation;

namespace Infrastructure.Services.Validations
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(i => i.Login)
                .EmailAddress().WithMessage("Invalid E-mail adress")
                .NotEmpty().WithMessage("Login cannot be empty");

            RuleFor(i => i.Name)
                .NotEmpty().WithMessage("Name cannot be empty")
                .Length(2, 32).WithMessage("Name length must be between 2 and 32")
                .Matches(@"^\w+$").WithMessage("Name can contain only letters");

            RuleFor(i => i.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Length(6, 32).WithMessage("Password length must be between 6 and 32")
                .Matches(@"[a-zA-Z0-9]{6,32}").WithMessage("Password can contain only letters");

            RuleFor(i => i.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password cannot be empty")
                .Equal(i => i.Password).WithMessage("The password and confirm password fields must match");
        }
    }
}
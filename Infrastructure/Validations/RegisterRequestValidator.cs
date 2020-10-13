using DTO.Request;
using FluentValidation;

namespace Infrastructure.Services.Validations
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(i => i.Login)
                .NotNull().WithMessage("Login cannot be empty")
                .Length(4, 10).WithMessage("Login length must be between 4 and 10")
                .Matches(@"^\w+$").WithMessage("Login can contain only letters");

            RuleFor(i => i.Name)
                .NotNull().WithMessage("Name cannot be empty")
                .Length(2, 10).WithMessage("Name length must be between 2 and 10")
                .Matches(@"^\w+$").WithMessage("Name can contain only letters");

            RuleFor(i => i.Password)
                .NotNull().WithMessage("Password cannot be empty")
                .Length(6, 10).WithMessage("Password length must be between 6 and 10")
                .Matches(@"[a-zA-Z0-9]{6,32}").WithMessage("Password can contain only letters");

            RuleFor(i => i.ConfirmPassword)
                .NotNull().WithMessage("Confirm password cannot be empty")
                .Equal(i => i.Password).WithMessage("The password and confirm password fields must match");
        }
    }
}
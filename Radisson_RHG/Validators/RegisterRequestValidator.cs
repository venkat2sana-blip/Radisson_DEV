using FluentValidation;
using Radisson_RHG.Models;

namespace Radisson_RHG.Validators
{
    public class RegisterRequestValidator: AbstractValidator<RegisterRequest>
    {

        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Length(5, 15);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("password must uppercase letter")
                .Matches("[0-9]").WithMessage("passowrd must contain numbers");
        }
    }
}

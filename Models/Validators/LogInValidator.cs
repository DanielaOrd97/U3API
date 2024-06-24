using FluentValidation;
using U3Api.Models.DTOs;

namespace U3Api.Models.Validators
{
    public class LogInValidator : AbstractValidator<LoginDto>
    {
        public LogInValidator()
        {
            RuleFor(x => x.User).NotEmpty().WithMessage("Ingrese su usuario.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Ingrese su contraseña.");
        }
    }
}

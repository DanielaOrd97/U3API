using FluentValidation;
using U3Api.Models.DTOs;

namespace U3Api.Models.Validators
{
    public class DepartamentoValidator: AbstractValidator<DepartamentoDTO>
    {
        public DepartamentoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Ingrese el nombre del departamento.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese un nombre de usuario.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Ingrese una contraseña.");
        }
    }
}

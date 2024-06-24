using Microsoft.AspNetCore.Mvc;
using U3Api.Models.DTOs;
using U3Api.Repositories;

namespace U3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DepartamentoRepository departamentoRepository;

        public LoginController(DepartamentoRepository departamentoRepository)
        {
            this.departamentoRepository = departamentoRepository;
        }

        [HttpPost]
        public IActionResult Post(LoginDto loginDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginDTO.User))
                    throw new Exception("El correo no debe ir vacio");

                if (string.IsNullOrWhiteSpace(loginDTO.Password))
                    throw new Exception("La contraseña no debe ir vacia");

                var depto = departamentoRepository.GetAll().FirstOrDefault(x => x.Username.Trim().ToUpper() == loginDTO.User.Trim().ToUpper()
                 && x.Password == loginDTO.Password);

                if (depto != null)
                    return Ok(new DepartamentoDTO()
                    {
                        Id = depto.Id,
                        IdSuperior = depto.IdSuperior,
                        Nombre = depto.Nombre,
                        Username = depto.Username
                    });
                else
                    return BadRequest("Nombre de usuario o contraseña incorrecta");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

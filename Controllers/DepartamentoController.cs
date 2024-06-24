using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using U3Api.Models.DTOs;
using U3Api.Repositories;
using U3API.Helpers;

namespace U3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly DepartamentoRepository departamentoRepository;

        public DepartamentoController(DepartamentoRepository departamentoRepository)
        {
            this.departamentoRepository = departamentoRepository;
        }

        static string CapitalizarCadaPalabra(string texto)
        {
            texto = texto.ToLower();
            TextInfo textInfo = new CultureInfo("es-MX", false).TextInfo;
            return textInfo.ToTitleCase(texto);
        }
        [HttpPost]
        public IActionResult Post(DepartamentoDTO departamento)
        {
            try
            {
                departamento.Id = 0;

                if (string.IsNullOrWhiteSpace(departamento.Nombre))
                    throw new Exception("El nombre del departamento no debe ir vacio");

                if (string.IsNullOrWhiteSpace(departamento.Username))
                    throw new Exception("El nombre de usuario del departamento no debe ir vacio");

                if (string.IsNullOrWhiteSpace(departamento.Password))
                    throw new Exception("El password de usuario del departamento no debe ir vacio");

                if (departamento.IdSuperior < 1)
                    throw new Exception("Debe tener un departamento padre");

                var dptos = departamentoRepository.GetAll();
                if (dptos.Any(x => x.Nombre.Trim().ToUpper() == departamento.Nombre.Trim().ToUpper()))
                    throw new Exception("Ya existe un departamento con ese nombre");

                if (dptos.Any(x => x.Username.Trim().ToUpper() == departamento.Username.Trim().ToUpper()))
                    throw new Exception("Ya existe ese nombre de usuario");

                var deptoSuperior = departamentoRepository.Get(departamento.IdSuperior);
                if (deptoSuperior == null)
                    throw new Exception("departamento superior no encontrado");

                //departamento.Nombre = CapitalizarCadaPalabra(departamento.Nombre.Trim());
                //departamento.Username = departamento.Username.Trim();


                departamentoRepository.Insert(departamento.ToEntity());

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        public IActionResult Get()
        {
            try
            {
                var deptos = departamentoRepository.GetAll().Select(x => new DepartamentoDTO()
                {
                    Id = x.Id,
                    IdSuperior = x.IdSuperior,
                    Nombre = x.Nombre,
                    Username = x.Username
                });
                return Ok(deptos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetDepartamentos(int id)
        {
            try
            {
                var deptos = departamentoRepository.GetDepartamentos(id);
                return Ok(deptos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put(DepartamentoDTO departamento)
        {
            try
            {

                if (departamento.Id == 0)
                    throw new Exception("Debe elegir un departamento a editar");

                if (string.IsNullOrWhiteSpace(departamento.Nombre))
                    throw new Exception("El nombre del departamento no debe ir vacio");

                if (string.IsNullOrWhiteSpace(departamento.Username))
                    throw new Exception("El nombre de usuario del departamento no debe ir vacio");


                var dptos = departamentoRepository.GetAll();
                if (dptos.Any(x => x.Nombre.Trim().ToUpper() == departamento.Nombre.Trim().ToUpper() && x.Id != departamento.Id))
                    throw new Exception("Ya existe un departamento con ese nombre");


                if (dptos.Any(x => x.Username.Trim().ToUpper() == departamento.Username.Trim().ToUpper() && x.Id != departamento.Id))
                    throw new Exception("Ya existe ese nombre de usuario");


                var depto = departamentoRepository.Get(departamento.Id);
                if (depto == null)
                    throw new Exception("departamento no encontrado");

                var deptoSuperior = departamentoRepository.Get(departamento.IdSuperior);
                if (deptoSuperior == null)
                    throw new Exception("departamento superior no encontrado");

                if (!string.IsNullOrWhiteSpace(departamento.Password))
                    depto.Password = departamento.Password;

                depto.Nombre = CapitalizarCadaPalabra(departamento.Nombre.Trim());
                depto.Username = departamento.Username.Trim();


                departamentoRepository.Update(depto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var depto = departamentoRepository.Get(id);
                if (depto == null)
                    throw new Exception("El departamento no existe o ya ha sido eliminado");

                departamentoRepository.Delete(depto);

                if(depto is null)
                {

                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_U3.Models.Validators;
using U3Api.Models.DTOs;
using U3Api.Repositories;
using U3API.Helpers;
using U3API.Models;

namespace U3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : ControllerBase
    {
        private readonly ActividadRepository actividadRepository;
        private readonly DepartamentoRepository departamentoRepository;
        private readonly IHttpClientFactory httpClientFactory;
        string rootpath;

        public ActividadController(ActividadRepository actividadRepository, DepartamentoRepository departamentoRepository,
            IWebHostEnvironment hostEnvironment, IHttpClientFactory httpClientFactory)
        {
            this.actividadRepository = actividadRepository;
            this.departamentoRepository = departamentoRepository;
            this.httpClientFactory = httpClientFactory;
            rootpath = hostEnvironment.WebRootPath;
        }

        string GetImage(int idActividad)
        {
            string host = HttpContext.Request.Host.Value;
            var imgpath = $"{rootpath}/imagenes/{idActividad}/evidencia.png";
            imgpath = imgpath.Replace("\\", "/");
            var path = "";
            if (System.IO.File.Exists(imgpath))
                path = $"https://{host}/imagenes/{idActividad}/evidencia.png";
            else
                path = "https://assets.gcore.pro/blog_containerizing_prod/uploads/2023/09/error-404-how-to-fix-it-fi.png";

            return path;
        }

        private void GuardarImagen(string imagen, int idActividad)
        {

            DeleteEvidence(idActividad);

            var directorio = rootpath + "/imagenes/" + idActividad;

            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            var bytesimg = Convert.FromBase64String(imagen);

            var rutadelaimagen = $"{directorio}/evidencia.png";



            System.IO.File.WriteAllBytes(rutadelaimagen.Replace("\\", "/"), bytesimg);
        }

        private void DeleteEvidence(int id)
        {
            string path = $"{rootpath}/imagenes/{id}/evidencia.png";

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        [HttpGet("departamento/{idDepartamento}")]
        public ActionResult GetActividades(int idDepartamento)
        {
            try
            {
                var depto = departamentoRepository.Get(idDepartamento);

                if (depto == null)
                    throw new Exception("Departamento no encontrado");

                var act = actividadRepository.GetActivities(idDepartamento);
                foreach (var subordinados in act.ActividadesSubordinadas)
                {
                    foreach (var a in subordinados.Actividades)
                    {
                        a.Evidencia = GetImage(a.Id);
                    }
                }

                return Ok(act);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Post(ActividadDTO actividad)
        {
            try
            {
                actividad.Id = 0;

                if (string.IsNullOrWhiteSpace(actividad.Titulo))
                    throw new Exception("El titulo de la actividad no debe ir vacío");

                if (string.IsNullOrWhiteSpace(actividad.Descripcion))
                    throw new Exception("La descripcion no debe ir vacia");

                if (!actividad.FechaRealizacion.HasValue)
                    throw new Exception("Debe especificar la fecha de realizacion");

                DateOnly ahora = DateOnly.FromDateTime(DateTime.Now.ToMexicoTime().Date);

                if (actividad.FechaRealizacion.HasValue && actividad.FechaRealizacion.Value > ahora)
                    throw new Exception("La fecha de realizacion no puede ser en el futuro");

                var depto = departamentoRepository.Get(actividad.IdDepartamento);

                if (depto == null)
                    throw new Exception("Departamento no encontrado");

                var a = actividad.ToEntity();
                a.FechaCreacion = DateTime.Now.ToMexicoTime();
                a.FechaActualizacion = DateTime.Now.ToMexicoTime();

                if (string.IsNullOrWhiteSpace(actividad.Evidencia))
                    throw new Exception("Debe enviar una evidencia");

                actividadRepository.Insert(a);

                GuardarImagen(actividad.Evidencia, a.Id);
                

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(ActividadDTO actividad)
        {
            try
            {
                var act = actividadRepository.Get(actividad.Id);

                if (act == null)
                    throw new Exception("Actividad no encontrada");

                if (string.IsNullOrWhiteSpace(actividad.Titulo))
                    throw new Exception("El titulo de la actividad no debe ir vacío");

                if (string.IsNullOrWhiteSpace(actividad.Descripcion))
                    throw new Exception("La descripcion no debe ir vacia");

                if (!actividad.FechaRealizacion.HasValue)
                    throw new Exception("Debe especificar la fecha de realizacion");

                DateOnly ahora = DateOnly.FromDateTime(DateTime.Now.ToMexicoTime().Date);

                if (actividad.FechaRealizacion.HasValue && actividad.FechaRealizacion.Value > ahora)
                    throw new Exception("La fecha de realizacion no puede ser en el futuro");

                var depto = departamentoRepository.Get(actividad.IdDepartamento);

                if (depto == null)
                    throw new Exception("Departamento no encontrado");

                act.FechaActualizacion = DateTime.Now.ToMexicoTime();
                act.Titulo = actividad.Titulo;
                act.Descripcion = actividad.Descripcion;
                act.FechaRealizacion = actividad.FechaRealizacion;
                act.Estado = actividad.Estado;



                actividadRepository.Update(act);

                if (!string.IsNullOrWhiteSpace(actividad.Evidencia))
                    GuardarImagen(actividad.Evidencia, act.Id);

                GetImage(actividad.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var act = actividadRepository.Get(id);
                if (act == null)
                    throw new Exception("Esta actividad no existe o ya ha sido eliminada");

                act.Estado = 2;
                actividadRepository.Update(act);

                DeleteEvidence(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}

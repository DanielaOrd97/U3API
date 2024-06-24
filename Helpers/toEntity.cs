using U3Api.Models.DTOs;
using U3API.Models;

namespace U3API.Helpers
{
    public static class toEntity
    {
        public static Departamentos ToEntity(this DepartamentoDTO departamento)
        {
            return new Departamentos()
            {
                Id = departamento.Id,
                IdSuperior = departamento.IdSuperior,
                Nombre = departamento.Nombre,
                Username = departamento.Username,
                Password = departamento.Password,
            };
        }

        public static Actividades ToEntity(this ActividadDTO actividades)
        {
            return new Actividades()
            {
                Id = actividades.Id,
                Descripcion = actividades.Descripcion,
                Estado = actividades.Estado,
                FechaActualizacion = actividades.FechaActualizacion,
                FechaCreacion = actividades.FechaCreacion,
                FechaRealizacion = actividades.FechaRealizacion,
                IdDepartamento = actividades.IdDepartamento,
                Titulo = actividades.Titulo
            };
        }
    }
}

using Microsoft.EntityFrameworkCore;
using U3Api.Models.DTOs;
using U3Api.Repositories;
using U3API.Models;

namespace U3Api.Repositories
{
    public class ActividadRepository : Repository<Actividades>
    {
        private readonly LabsysteDoubledContext context;

        public ActividadRepository(LabsysteDoubledContext context) : base(context)
        {
            this.context = context;
        }

        public override IEnumerable<Actividades> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Titulo);
        }

        public MisActividadesYSubordinados GetActivities(int idDepartamento)
        {
            var depto = context.Departamentos.Include(x => x.Actividades.Where(x=>x.Estado==0 || x.Estado==1)).Include(x => x.InverseIdSuperiorNavigation)
                .ThenInclude(x => x.Actividades.Where(x=>x.Estado==1)).First(x => x.Id == idDepartamento);

            MisActividadesYSubordinados misActividadesYSubordinados = new();


            var Misact = depto.Actividades.Select(x =>
            new ActividadDTO()
            {
                Id = x.Id,
                Descripcion = x.Descripcion,
                IdDepartamento = x.IdDepartamento,
                Estado = x.Estado,
                FechaActualizacion = x.FechaActualizacion,
                FechaCreacion = x.FechaCreacion,
                FechaRealizacion = x.FechaRealizacion,
                Titulo = x.Titulo
            }).OrderByDescending(x=>x.FechaRealizacion).ToList();

            //misActividadesYSubordinados.MisActividades = Misact;
            ActividadesSubordinadas actividadSubordinada = new();
            actividadSubordinada.IdDepartamento = depto.Id;
            actividadSubordinada.NombreDepartamento = depto.Nombre + "(Usuario Actual)";
            actividadSubordinada.Actividades = Misact;

            misActividadesYSubordinados.ActividadesSubordinadas.Add(actividadSubordinada);
            foreach (var deptoHijo in depto.InverseIdSuperiorNavigation)
            {
                actividadSubordinada = new();
                actividadSubordinada.IdDepartamento = deptoHijo.Id;
                actividadSubordinada.NombreDepartamento = deptoHijo.Nombre;
                actividadSubordinada.Actividades = deptoHijo.Actividades.Select(x =>
                new ActividadDTO()
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    IdDepartamento = x.IdDepartamento,
                    Estado = x.Estado,
                    FechaActualizacion = x.FechaActualizacion,
                    FechaCreacion = x.FechaCreacion,
                    FechaRealizacion = x.FechaRealizacion,
                    Titulo = x.Titulo
                }).OrderByDescending(x => x.FechaRealizacion).ToList();

                misActividadesYSubordinados.ActividadesSubordinadas.Add(actividadSubordinada);
            }

            misActividadesYSubordinados.ActividadesSubordinadas =
            misActividadesYSubordinados.ActividadesSubordinadas.OrderBy(x => x.NombreDepartamento).ToList();

            return misActividadesYSubordinados;

        }
    }
}

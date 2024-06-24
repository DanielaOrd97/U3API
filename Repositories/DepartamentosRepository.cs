using Microsoft.EntityFrameworkCore;
using U3Api.Models.DTOs;
using U3API.Models;

namespace U3Api.Repositories
{
    public class DepartamentoRepository : Repository<Departamentos>
    {
        private readonly LabsysteDoubledContext context;

        public DepartamentoRepository(LabsysteDoubledContext context) : base(context)
        {
            this.context = context;
        }

        public override IEnumerable<Departamentos> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Nombre);
        }

        public IEnumerable<DepartamentoDTO> GetDepartamentos(int id)
        {
            var depto = context.Departamentos.Include(x => x.InverseIdSuperiorNavigation).FirstOrDefault(x => x.Id == id);

            if (depto != null)
            {
                List<DepartamentoDTO> deptos = new()
                {
                    new DepartamentoDTO()
                    {
                        Id = depto.Id,
                        Nombre = depto.Nombre + " (Usuario Actual)",
                        Username = depto.Username,
                        IdSuperior= depto.IdSuperior
                    }
                };

                foreach (var dep in depto.InverseIdSuperiorNavigation)
                {
                    deptos.Add(new DepartamentoDTO()
                    {
                        Id = dep.Id,
                        Nombre = dep.Nombre,
                        IdSuperior = dep.IdSuperior,
                        Username = dep.Username
                    });
                }

                return deptos;
            }
            else

                return Enumerable.Empty<DepartamentoDTO>();

        }

        public override void Delete(Departamentos id)
        {
            var departamento = context.Departamentos
                                      .Include(d => d.Actividades)
                                      .First(d => d.Id == id.Id);

            if (departamento.Actividades.Any() &&
                !departamento.Actividades.All(a => a.Estado == 2))
            {
                throw new InvalidOperationException("El departamento no se puede eliminar porque está asociado con actividades existentes que no están en estado 2.");
            }

            var actividadesParaEliminar = departamento.Actividades
                                                      .Where(a => a.Estado == 2)
                                                      .ToList();
            context.Actividades.RemoveRange(actividadesParaEliminar);
            context.SaveChanges();

            base.Delete(id);
        }

        //public override void Delete(Departamentos id)
        //{
        //    //bool indicador = false;
        //    var departamento = context.Departamentos.Include(d => d.Actividades).First(d => d.Id == id.Id);

        //    //VERIFICAR QUE LAS ACT ESTEN EN ESTADO 2 PARA ELIMINAR DEPTO
        //    //foreach (var item in departamento.Actividades)
        //    //{
        //    //    if(item.Estado != 2)
        //    //    {
        //    //        indicador = true;
        //    //    }
        //    //}

        //    //if(indicador == true)
        //    //{
        //    //    throw new InvalidOperationException("El departamento no se puede eliminar porque está asociado con actividades existentes.");
        //    //}
        //    //else
        //    //{

        //    //    //base.Delete(id);
        //    //}

        //    if (departamento.Actividades.Any())
        //    {
        //        throw new InvalidOperationException("El departamento no se puede eliminar porque está asociado con actividades existentes.");
        //    }

        //    base.Delete(id);


        //}
    }
}


namespace U3Api.Models.DTOs
{
    public class ActividadDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; } = null!;
        public DateOnly? FechaRealizacion { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int Estado { get; set; }
        public string Evidencia { get; set; } = null!;

    }
    public class ActividadesSubordinadas
    {
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; } = "";
        public List<ActividadDTO> Actividades { get; set; } = new();
    }

    public class MisActividadesYSubordinados
    {
        public List<ActividadesSubordinadas> ActividadesSubordinadas { get; set; } = new();
    }
}

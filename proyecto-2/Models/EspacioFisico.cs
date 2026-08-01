
namespace proyecto_2.Models
{
    public class EspacioFisico
    {
        public int EspacioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int CapacidadMaxima { get; set; }
    }
}
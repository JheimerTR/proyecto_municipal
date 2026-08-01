
namespace proyecto_1.Models
{
    public class Asistencia
    {
        public int AsistenciaID { get; set; }
        public int InscripcionID { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int RegistradoPor { get; set;}
    }
}
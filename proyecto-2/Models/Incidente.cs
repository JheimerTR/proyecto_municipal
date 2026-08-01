
namespace proyecto_2.Models
{
    public class Incidente
    {
        public int IncidenteID { get; set; }
        public int ParticipanteID { get; set; }
        public int CursoID { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int RegistradoPor { get; set; }
    }
}
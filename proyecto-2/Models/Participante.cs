
namespace proyecto_2.Models
{
    public class Participante
    {
        public int ParticipanteID { get; set; }
        public int TutorID { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string? Alergias { get; set; }
        public string? ContactoEmergenciaNombre { get; set; }
        public string? ContactoEmergenciaTelefono { get; set; }
    }
}
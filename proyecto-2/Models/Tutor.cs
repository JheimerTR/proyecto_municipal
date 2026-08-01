
namespace proyecto_2.Models
{
    public class Tutor
    {
        public int TutorID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
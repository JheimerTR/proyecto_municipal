
namespace proyecto_2.Models
{
    public class Curso
    {
        public int CursoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CupoMaximo { get; set; }
        public int InstructorID { get; set; }
        public int EspacioID { get; set; }
        public int HorarioID { get; set; }
    }
}
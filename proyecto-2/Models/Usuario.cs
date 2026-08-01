
namespace proyecto_2.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string UsuarioRed { get; set; } = string.Empty;
        public string Contrasena { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public byte Activo { get; set; } = 1;
    }
}
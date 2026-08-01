using Microsoft.AspNetCore.Mvc;
using proyecto_2.Models;
using proyecto_2.Repositorio; // Asegúrate de importar el namespace de tu repositorio

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly CursoRepository _repositorio;

        public CursoController(IConfiguration config)
        {
            // Extraemos la cadena de conexión
            string cadenaConexion = config.GetConnectionString("DefaultConnection");

            // Instanciamos el repositorio para que el controlador lo use
            _repositorio = new CursoRepository(cadenaConexion);
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            // El controlador solo llama al repositorio, él se encarga de MySQL
            var lista = _repositorio.ObtenerTodos();
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Curso curso)
        {
            try
            {
                // Aquí se ejecuta el INSERT y las validaciones de choque de horarios
                _repositorio.Insertar(curso);
                return Ok(new { mensaje = "Curso creado exitosamente." });
            }
            catch (Exception ex)
            {
                // Si el repositorio lanza la excepción de choque de horario, la atrapamos aquí
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Curso curso)
        {
            try
            {
                // Aseguramos que el ID del objeto coincida con el de la URL
                curso.CursoID = id;
                _repositorio.Actualizar(curso); // Asegúrate de crear este método en tu CursoRepository
                return Ok(new { mensaje = "Curso actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _repositorio.Eliminar(id); // Asegúrate de crear este método en tu CursoRepository
                return Ok(new { mensaje = "Curso eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
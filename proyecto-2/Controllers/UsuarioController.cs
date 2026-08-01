using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using proyecto_2.Models;
using proyecto_2.Repositories; // O proyecto_2.Repositorio, usa tu nombre de carpeta
using System;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _repositorio;

        // Inyectamos la configuración para leer del appsettings.json
        public UsuarioController(IConfiguration config)
        {
            // Asumo que tu cadena en appsettings.json se llama "DefaultConnection"
            string cadenaConexion = config.GetConnectionString("DefaultConnection");
            _repositorio = new UsuarioRepository(cadenaConexion);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lista = _repositorio.ObtenerTodos();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _repositorio.ObtenerPorId(id);
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            try
            {
                bool exito = _repositorio.Crear(usuario);
                if (exito) return Ok(new { mensaje = "Usuario creado exitosamente" });
                return BadRequest(new { error = "No se pudo crear el usuario" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                usuario.UsuarioID = id; // Aseguramos que el ID coincida
                bool exito = _repositorio.Editar(usuario);
                if (exito) return Ok(new { mensaje = "Usuario actualizado exitosamente" });
                return BadRequest(new { error = "No se pudo actualizar el usuario" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool exito = _repositorio.Eliminar(id);
                if (exito) return Ok(new { mensaje = "Usuario eliminado exitosamente" });
                return BadRequest(new { error = "No se pudo eliminar el usuario" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuarioAutenticado = _repositorio.AutenticarLogin(request.UsuarioRed, request.Contrasena);

                if (usuarioAutenticado != null)
                {
                    return Ok(usuarioAutenticado); // Retorna HTTP 200 con los datos
                }

                return Unauthorized(new { mensaje = "Credenciales incorrectas o usuario inactivo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error en el servidor: " + ex.Message });
            }
        }
    }
}
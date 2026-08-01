using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using proyecto_2.Models;
using proyecto_2.Repositorio;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly TutorRepository _repositorio;

        // Aquí inyectamos la configuración para leer la cadena y pasársela al Repositorio
        public TutorController(IConfiguration config)
        {
            string cadenaConexion = config.GetConnectionString("DefaultConnection");
            _repositorio = new TutorRepository(cadenaConexion);
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            try
            {
                var lista = _repositorio.ObtenerTodos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error en la base de datos: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Tutor tutor)
        {
            try
            {
                _repositorio.Insertar(tutor);
                return Ok(new { mensaje = "Tutor creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al guardar: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Tutor tutor)
        {
            try
            {
                _repositorio.Actualizar(id, tutor);
                return Ok(new { mensaje = "Tutor actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _repositorio.Eliminar(id);
                return Ok(new { mensaje = "Tutor eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar: " + ex.Message });
            }
        }
    }
}
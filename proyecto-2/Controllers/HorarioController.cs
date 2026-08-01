using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using proyecto_2.Models;
using proyecto_2.Repositorio;
using System;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly HorarioRepository _repositorio;

        public HorarioController(IConfiguration config)
        {
            _repositorio = new HorarioRepository(config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            try
            {
                return Ok(_repositorio.ObtenerTodos());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error en BD: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Horario horario)
        {
            try
            {
                _repositorio.Insertar(horario);
                return Ok(new { mensaje = "Horario creado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al insertar: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Horario horario)
        {
            try
            {
                _repositorio.Actualizar(id, horario);
                return Ok(new { mensaje = "Horario actualizado correctamente." });
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
                return Ok(new { mensaje = "Horario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar: " + ex.Message });
            }
        }
    }
}
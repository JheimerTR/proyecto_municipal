using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using proyecto_2.Models;
using proyecto_2.Repositorio;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly InscripcionRepository _repositorio;

        public InscripcionController(IConfiguration config)
        {
            _repositorio = new InscripcionRepository(config.GetConnectionString("DefaultConnection"));
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
                return StatusCode(500, new { error = "Error BD: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Inscripcion inscripcion)
        {
            try
            {
                int numeroTicket = _repositorio.Insertar(inscripcion);

                return Ok(new
                {
                    mensaje = "Inscripción registrada con éxito.",
                    ticket = new
                    {
                        numeroInscripcion = numeroTicket,
                        participanteID = inscripcion.ParticipanteID,
                        cursoID = inscripcion.CursoID,
                        fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al insertar: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Inscripcion inscripcion)
        {
            try
            {
                _repositorio.Actualizar(id, inscripcion);
                return Ok(new { mensaje = "Inscripción actualizada correctamente." });
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
                return Ok(new { mensaje = "Inscripción eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar: " + ex.Message });
            }
        }
    }
}
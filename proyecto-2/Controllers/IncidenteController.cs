using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using proyecto_2.Models;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidenteController : ControllerBase
    {
        private readonly string _cadenaConexion;

        public IncidenteController(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            List<Incidente> lista = new List<Incidente>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Incidente", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Incidente
                        {
                            IncidenteID = Convert.ToInt32(dr["IncidenteID"]),
                            ParticipanteID = Convert.ToInt32(dr["ParticipanteID"]),
                            CursoID = Convert.ToInt32(dr["CursoID"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Descripcion = dr["Descripcion"].ToString(),
                            RegistradoPor = Convert.ToInt32(dr["RegistradoPor"])
                        });
                    }
                }
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Incidente incidente)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "INSERT INTO Incidente (ParticipanteID, CursoID, Descripcion, RegistradoPor) VALUES (@ParticipanteID, @CursoID, @Descripcion, @RegistradoPor)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ParticipanteID", incidente.ParticipanteID);
                    cmd.Parameters.AddWithValue("@CursoID", incidente.CursoID);
                    cmd.Parameters.AddWithValue("@Descripcion", incidente.Descripcion);
                    cmd.Parameters.AddWithValue("@RegistradoPor", incidente.RegistradoPor);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Incidente reportado correctamente." });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Incidente incidente)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "UPDATE Incidente SET ParticipanteID=@ParticipanteID, CursoID=@CursoID, Descripcion=@Descripcion, RegistradoPor=@RegistradoPor WHERE IncidenteID=@Id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ParticipanteID", incidente.ParticipanteID);
                    cmd.Parameters.AddWithValue("@CursoID", incidente.CursoID);
                    cmd.Parameters.AddWithValue("@Descripcion", incidente.Descripcion);
                    cmd.Parameters.AddWithValue("@RegistradoPor", incidente.RegistradoPor);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Incidente actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Incidente WHERE IncidenteID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            return Ok(new { mensaje = "Incidente eliminado correctamente." });
        }
    }
}
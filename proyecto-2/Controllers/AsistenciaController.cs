using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using proyecto_1.Models;
using proyecto_2.Models;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private readonly string _cadenaConexion;

        public AsistenciaController(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            List<Asistencia> lista = new List<Asistencia>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Asistencia", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Asistencia
                        {
                            AsistenciaID = Convert.ToInt32(dr["AsistenciaID"]),
                            InscripcionID = Convert.ToInt32(dr["InscripcionID"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Estado = dr["Estado"].ToString(),
                            RegistradoPor = Convert.ToInt32(dr["RegistradoPor"])
                        });
                    }
                }
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Asistencia asistencia)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "INSERT INTO Asistencia (InscripcionID, Fecha, Estado, RegistradoPor) VALUES (@InscripcionID, @Fecha, @Estado, @RegistradoPor)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@InscripcionID", asistencia.InscripcionID);
                    cmd.Parameters.AddWithValue("@Fecha", asistencia.Fecha);
                    cmd.Parameters.AddWithValue("@Estado", asistencia.Estado);
                    cmd.Parameters.AddWithValue("@RegistradoPor", asistencia.RegistradoPor);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Asistencia registrada correctamente." });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Asistencia asistencia)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "UPDATE Asistencia SET InscripcionID=@InscripcionID, Fecha=@Fecha, Estado=@Estado, RegistradoPor=@RegistradoPor WHERE AsistenciaID=@Id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@InscripcionID", asistencia.InscripcionID);
                    cmd.Parameters.AddWithValue("@Fecha", asistencia.Fecha);
                    cmd.Parameters.AddWithValue("@Estado", asistencia.Estado);
                    cmd.Parameters.AddWithValue("@RegistradoPor", asistencia.RegistradoPor);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Asistencia actualizada correctamente." });
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Asistencia WHERE AsistenciaID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            return Ok(new { mensaje = "Asistencia eliminada correctamente." });
        }
    }
}
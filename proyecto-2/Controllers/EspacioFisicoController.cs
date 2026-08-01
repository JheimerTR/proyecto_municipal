using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using proyecto_2.Models;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspacioFisicoController : ControllerBase
    {
        private readonly string _cadenaConexion;

        public EspacioFisicoController(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            List<EspacioFisico> lista = new List<EspacioFisico>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM EspacioFisico", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EspacioFisico
                        {
                            EspacioID = Convert.ToInt32(dr["EspacioID"]),
                            Nombre = dr["Nombre"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            CapacidadMaxima = Convert.ToInt32(dr["CapacidadMaxima"])
                        });
                    }
                }
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] EspacioFisico espacio)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "INSERT INTO EspacioFisico (Nombre, Tipo, CapacidadMaxima) VALUES (@Nombre, @Tipo, @CapacidadMaxima)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", espacio.Nombre);
                    cmd.Parameters.AddWithValue("@Tipo", espacio.Tipo);
                    cmd.Parameters.AddWithValue("@CapacidadMaxima", espacio.CapacidadMaxima);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Espacio Físico creado correctamente." });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] EspacioFisico espacio)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "UPDATE EspacioFisico SET Nombre=@Nombre, Tipo=@Tipo, CapacidadMaxima=@CapacidadMaxima WHERE EspacioID=@Id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nombre", espacio.Nombre);
                    cmd.Parameters.AddWithValue("@Tipo", espacio.Tipo);
                    cmd.Parameters.AddWithValue("@CapacidadMaxima", espacio.CapacidadMaxima);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Espacio Físico actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM EspacioFisico WHERE EspacioID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            return Ok(new { mensaje = "Espacio Físico eliminado correctamente." });
        }
    }
}
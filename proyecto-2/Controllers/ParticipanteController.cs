using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using proyecto_2.Models;

namespace proyecto_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipanteController : ControllerBase
    {
        private readonly string _cadenaConexion;

        public ParticipanteController(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            List<Participante> lista = new List<Participante>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Participante", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Participante
                        {
                            ParticipanteID = Convert.ToInt32(dr["ParticipanteID"]),
                            TutorID = Convert.ToInt32(dr["TutorID"]),
                            Identificacion = dr["Identificacion"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                            Alergias = dr["Alergias"].ToString(),
                            ContactoEmergenciaNombre = dr["ContactoEmergenciaNombre"].ToString(),
                            ContactoEmergenciaTelefono = dr["ContactoEmergenciaTelefono"].ToString()
                        });
                    }
                }
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Insertar([FromBody] Participante participante)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "INSERT INTO Participante (TutorID, Identificacion, Nombre, FechaNacimiento, Alergias, ContactoEmergenciaNombre, ContactoEmergenciaTelefono) VALUES (@TutorID, @Identificacion, @Nombre, @FechaNacimiento, @Alergias, @ContactoEmergenciaNombre, @ContactoEmergenciaTelefono)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TutorID", participante.TutorID);
                    cmd.Parameters.AddWithValue("@Identificacion", participante.Identificacion);
                    cmd.Parameters.AddWithValue("@Nombre", participante.Nombre);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", participante.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Alergias", participante.Alergias);
                    cmd.Parameters.AddWithValue("@ContactoEmergenciaNombre", participante.ContactoEmergenciaNombre);
                    cmd.Parameters.AddWithValue("@ContactoEmergenciaTelefono", participante.ContactoEmergenciaTelefono);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Participante creado correctamente." });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Participante participante)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "UPDATE Participante SET TutorID=@TutorID, Identificacion=@Identificacion, Nombre=@Nombre, FechaNacimiento=@FechaNacimiento, Alergias=@Alergias, ContactoEmergenciaNombre=@ContactoEmergenciaNombre, ContactoEmergenciaTelefono=@ContactoEmergenciaTelefono WHERE ParticipanteID=@Id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@TutorID", participante.TutorID);
                    cmd.Parameters.AddWithValue("@Identificacion", participante.Identificacion);
                    cmd.Parameters.AddWithValue("@Nombre", participante.Nombre);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", participante.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Alergias", participante.Alergias);
                    cmd.Parameters.AddWithValue("@ContactoEmergenciaNombre", participante.ContactoEmergenciaNombre);
                    cmd.Parameters.AddWithValue("@ContactoEmergenciaTelefono", participante.ContactoEmergenciaTelefono);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { mensaje = "Participante actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Participante WHERE ParticipanteID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            return Ok(new { mensaje = "Participante eliminado correctamente." });
        }
    }
}
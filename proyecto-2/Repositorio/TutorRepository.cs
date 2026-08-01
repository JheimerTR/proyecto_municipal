using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using proyecto_2.Models;

namespace proyecto_2.Repositorio
{
    public class TutorRepository
    {
        private readonly string _cadenaConexion;

        public TutorRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<Tutor> ObtenerTodos()
        {
            List<Tutor> lista = new List<Tutor>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Tutor", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Tutor
                        {
                            TutorID = Convert.ToInt32(dr["TutorID"]),
                            Nombre = dr["Nombre"].ToString(),
                            Identificacion = dr["Identificacion"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Email = dr["Email"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public void Insertar(Tutor tutor)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "INSERT INTO Tutor (Nombre, Identificacion, Telefono, Email) VALUES (@Nombre, @Identificacion, @Telefono, @Email)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", tutor.Nombre);
                    cmd.Parameters.AddWithValue("@Identificacion", tutor.Identificacion);
                    cmd.Parameters.AddWithValue("@Telefono", tutor.Telefono);
                    // Aquí manejamos el nulo de forma segura
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(tutor.Email) ? (object)DBNull.Value : tutor.Email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(int id, Tutor tutor)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = "UPDATE Tutor SET Nombre=@Nombre, Identificacion=@Identificacion, Telefono=@Telefono, Email=@Email WHERE TutorID=@Id";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nombre", tutor.Nombre);
                    cmd.Parameters.AddWithValue("@Identificacion", tutor.Identificacion);
                    cmd.Parameters.AddWithValue("@Telefono", tutor.Telefono);
                    // Aquí también manejamos el nulo
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(tutor.Email) ? (object)DBNull.Value : tutor.Email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Tutor WHERE TutorID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
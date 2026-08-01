using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using proyecto_2.Models;

namespace proyecto_2.Repositorio
{
    public class InscripcionRepository
    {
        private readonly string _cadenaConexion;

        public InscripcionRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<Inscripcion> ObtenerTodos()
        {
            List<Inscripcion> lista = new List<Inscripcion>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM Inscripcion", con))
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Inscripcion
                        {
                            InscripcionID = Convert.ToInt32(dr["InscripcionID"]),
                            ParticipanteID = Convert.ToInt32(dr["ParticipanteID"]),
                            CursoID = Convert.ToInt32(dr["CursoID"]),
                            FechaInscripcion = Convert.ToDateTime(dr["FechaInscripcion"]),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public int Insertar(Inscripcion inscripcion)
        {
            int idGenerado = 0;
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = @"INSERT INTO Inscripcion (ParticipanteID, CursoID, FechaInscripcion, Estado) 
                                 VALUES (@ParticipanteID, @CursoID, @FechaInscripcion, @Estado);
                                 SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ParticipanteID", inscripcion.ParticipanteID);
                    cmd.Parameters.AddWithValue("@CursoID", inscripcion.CursoID);
                    cmd.Parameters.AddWithValue("@FechaInscripcion", inscripcion.FechaInscripcion == default ? DateTime.Now : inscripcion.FechaInscripcion);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrEmpty(inscripcion.Estado) ? "Activa" : inscripcion.Estado);

                    idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return idGenerado;
        }

        public void Actualizar(int id, Inscripcion inscripcion)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = @"UPDATE Inscripcion 
                                 SET ParticipanteID=@ParticipanteID, CursoID=@CursoID, FechaInscripcion=@FechaInscripcion, Estado=@Estado 
                                 WHERE InscripcionID=@Id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@ParticipanteID", inscripcion.ParticipanteID);
                    cmd.Parameters.AddWithValue("@CursoID", inscripcion.CursoID);
                    cmd.Parameters.AddWithValue("@FechaInscripcion", inscripcion.FechaInscripcion);
                    cmd.Parameters.AddWithValue("@Estado", inscripcion.Estado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Inscripcion WHERE InscripcionID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
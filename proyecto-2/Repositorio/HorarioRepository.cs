using MySql.Data.MySqlClient;

using proyecto_2.Models;
using System;
using System.Collections.Generic;

namespace proyecto_2.Repositorio
{
    public class HorarioRepository
    {
        private readonly string _cadenaConexion;

        public HorarioRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<Horario> ObtenerTodos()
        {
            List<Horario> lista = new List<Horario>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Horario", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Horario
                        {
                            HorarioID = Convert.ToInt32(dr["HorarioID"]),
                            // Corregido a DiaSemana según tu base de datos
                            DiaSemana = dr["DiaSemana"].ToString(),
                            HoraInicio = TimeSpan.Parse(dr["HoraInicio"].ToString()),
                            HoraFin = TimeSpan.Parse(dr["HoraFin"].ToString())
                        });
                    }
                }
            }
            return lista;
        }

        public void Insertar(Horario horario)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                // Corregido a DiaSemana
                string query = "INSERT INTO Horario (DiaSemana, HoraInicio, HoraFin) VALUES (@DiaSemana, @HoraInicio, @HoraFin)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DiaSemana", horario.DiaSemana);
                    cmd.Parameters.AddWithValue("@HoraInicio", horario.HoraInicio);
                    cmd.Parameters.AddWithValue("@HoraFin", horario.HoraFin);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(int id, Horario horario)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                // Corregido a DiaSemana
                string query = "UPDATE Horario SET DiaSemana=@DiaSemana, HoraInicio=@HoraInicio, HoraFin=@HoraFin WHERE HorarioID=@Id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@DiaSemana", horario.DiaSemana);
                    cmd.Parameters.AddWithValue("@HoraInicio", horario.HoraInicio);
                    cmd.Parameters.AddWithValue("@HoraFin", horario.HoraFin);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Horario WHERE HorarioID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
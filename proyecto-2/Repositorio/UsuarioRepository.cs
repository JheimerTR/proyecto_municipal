using MySql.Data.MySqlClient;
using proyecto_2.Models;
using System;
using System.Collections.Generic;

namespace proyecto_2.Repositories // Asegúrate de que este namespace coincida con tu carpeta (Repositories o Repositorio)
{
    public class UsuarioRepository
    {
        private readonly string _cadenaConexion;

        // Le pasamos la cadena de conexión por el constructor, NO la quemamos aquí
        public UsuarioRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> lista = new List<Usuario>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Usuario", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Usuario
                        {
                            UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                            UsuarioRed = dr["UsuarioRed"].ToString() ?? string.Empty,
                            Contrasena = dr["Contrasena"].ToString() ?? string.Empty,
                            NombreCompleto = dr["NombreCompleto"].ToString() ?? string.Empty,
                            Rol = dr["Rol"].ToString() ?? string.Empty,
                            Activo = Convert.ToByte(dr["Activo"])
                        });
                    }
                }
            }
            return lista;
        }

        public Usuario ObtenerPorId(int id)
        {
            Usuario usuario = new Usuario();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Usuario WHERE UsuarioID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        usuario = new Usuario
                        {
                            UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                            UsuarioRed = dr["UsuarioRed"].ToString() ?? string.Empty,
                            Contrasena = dr["Contrasena"].ToString() ?? string.Empty,
                            NombreCompleto = dr["NombreCompleto"].ToString() ?? string.Empty,
                            Rol = dr["Rol"].ToString() ?? string.Empty,
                            Activo = Convert.ToByte(dr["Activo"])
                        };
                    }
                }
            }
            return usuario;
        }

        public bool Crear(Usuario usuario)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO Usuario (UsuarioRed, Contrasena, NombreCompleto, Rol, Activo) " +
                    "VALUES (@usuarioRed, @contrasena, @nombreCompleto, @rol, @activo)", con);

                cmd.Parameters.AddWithValue("@usuarioRed", usuario.UsuarioRed);
                cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                cmd.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
                cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                cmd.Parameters.AddWithValue("@activo", usuario.Activo);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Editar(Usuario usuario)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE Usuario SET UsuarioRed = @usuarioRed, Contrasena = @contrasena, " +
                    "NombreCompleto = @nombreCompleto, Rol = @rol, Activo = @activo " +
                    "WHERE UsuarioID = @id", con);

                cmd.Parameters.AddWithValue("@id", usuario.UsuarioID);
                cmd.Parameters.AddWithValue("@usuarioRed", usuario.UsuarioRed);
                cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                cmd.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
                cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                cmd.Parameters.AddWithValue("@activo", usuario.Activo);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Usuario WHERE UsuarioID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Lógica de Login que antes estaba en el controlador, ahora vive aquí
        public object AutenticarLogin(string usuarioRed, string contrasena)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                string query = @"SELECT UsuarioID, NombreCompleto, Rol 
                                 FROM Usuario 
                                 WHERE UsuarioRed = @UsuarioRed 
                                 AND Contrasena = @Contrasena 
                                 AND Activo = 1";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UsuarioRed", usuarioRed);
                    cmd.Parameters.AddWithValue("@Contrasena", contrasena);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new
                            {
                                UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                                NombreCompleto = reader["NombreCompleto"].ToString(),
                                Rol = reader["Rol"].ToString()
                            };
                        }
                        return null; // Retorna null si las credenciales son incorrectas
                    }
                }
            }
        }
    }
}
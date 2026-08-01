using MySql.Data.MySqlClient;
using proyecto_2.Models;
using System.Collections.Generic;

namespace proyecto_2.Repositorio
{
    public class CursoRepository
    {
        private readonly string _cadenaConexion;

        public CursoRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        // Método para obtener todos los cursos (GET)
        public List<Curso> ObtenerTodos()
        {
            List<Curso> lista = new List<Curso>();
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Curso", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Curso
                        {
                            CursoID = Convert.ToInt32(dr["CursoID"]),
                            Nombre = dr["Nombre"].ToString(),
                            Categoria = dr["Categoria"].ToString(),
                            CupoMaximo = Convert.ToInt32(dr["CupoMaximo"]),
                            InstructorID = Convert.ToInt32(dr["InstructorID"]),
                            EspacioID = Convert.ToInt32(dr["EspacioID"]),
                            HorarioID = Convert.ToInt32(dr["HorarioID"])
                        });
                    }
                }
            }
            return lista;
        }

        // Método para insertar un curso (POST)
        // Método para insertar un curso (POST) con validaciones
        public void Insertar(Curso curso)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();

                // 1. Validar Choque de Horario para el Instructor
                string queryInstructor = "SELECT COUNT(*) FROM Curso WHERE InstructorID = @InstructorID AND HorarioID = @HorarioID";
                using (MySqlCommand cmdInstructor = new MySqlCommand(queryInstructor, con))
                {
                    cmdInstructor.Parameters.AddWithValue("@InstructorID", curso.InstructorID);
                    cmdInstructor.Parameters.AddWithValue("@HorarioID", curso.HorarioID);
                    int countInstructor = Convert.ToInt32(cmdInstructor.ExecuteScalar());

                    if (countInstructor > 0)
                    {
                        // Detenemos la ejecución y lanzamos el error al controlador
                        throw new Exception("El instructor seleccionado ya tiene un curso asignado en este horario.");
                    }
                }

                // 2. Validar Choque de Horario para el Espacio Físico (Aula)
                string queryEspacio = "SELECT COUNT(*) FROM Curso WHERE EspacioID = @EspacioID AND HorarioID = @HorarioID";
                using (MySqlCommand cmdEspacio = new MySqlCommand(queryEspacio, con))
                {
                    cmdEspacio.Parameters.AddWithValue("@EspacioID", curso.EspacioID);
                    cmdEspacio.Parameters.AddWithValue("@HorarioID", curso.HorarioID);
                    int countEspacio = Convert.ToInt32(cmdEspacio.ExecuteScalar());

                    if (countEspacio > 0)
                    {
                        // Detenemos la ejecución
                        throw new Exception("El espacio físico seleccionado ya está ocupado en este horario.");
                    }
                }

                // 3. Si pasa las validaciones (count == 0), procedemos con el INSERT
                string query = @"INSERT INTO Curso (Nombre, Categoria, CupoMaximo, InstructorID, EspacioID, HorarioID) 
                         VALUES (@Nombre, @Categoria, @CupoMaximo, @InstructorID, @EspacioID, @HorarioID)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", curso.Nombre);
                    cmd.Parameters.AddWithValue("@Categoria", curso.Categoria);
                    cmd.Parameters.AddWithValue("@CupoMaximo", curso.CupoMaximo);
                    cmd.Parameters.AddWithValue("@InstructorID", curso.InstructorID);
                    cmd.Parameters.AddWithValue("@EspacioID", curso.EspacioID);
                    cmd.Parameters.AddWithValue("@HorarioID", curso.HorarioID);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Método para actualizar un curso (PUT) con validaciones
        public void Actualizar(Curso curso)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();

                // 1. Validar Choque de Horario para el Instructor (ignorando el curso actual)
                string queryInstructor = "SELECT COUNT(*) FROM Curso WHERE InstructorID = @InstructorID AND HorarioID = @HorarioID AND CursoID != @CursoID";
                using (MySqlCommand cmdInstructor = new MySqlCommand(queryInstructor, con))
                {
                    cmdInstructor.Parameters.AddWithValue("@InstructorID", curso.InstructorID);
                    cmdInstructor.Parameters.AddWithValue("@HorarioID", curso.HorarioID);
                    cmdInstructor.Parameters.AddWithValue("@CursoID", curso.CursoID);

                    if (Convert.ToInt32(cmdInstructor.ExecuteScalar()) > 0)
                    {
                        throw new Exception("El instructor ya tiene otro curso asignado en este horario.");
                    }
                }

                // 2. Validar Choque de Horario para el Espacio Físico (ignorando el curso actual)
                string queryEspacio = "SELECT COUNT(*) FROM Curso WHERE EspacioID = @EspacioID AND HorarioID = @HorarioID AND CursoID != @CursoID";
                using (MySqlCommand cmdEspacio = new MySqlCommand(queryEspacio, con))
                {
                    cmdEspacio.Parameters.AddWithValue("@EspacioID", curso.EspacioID);
                    cmdEspacio.Parameters.AddWithValue("@HorarioID", curso.HorarioID);
                    cmdEspacio.Parameters.AddWithValue("@CursoID", curso.CursoID);

                    if (Convert.ToInt32(cmdEspacio.ExecuteScalar()) > 0)
                    {
                        throw new Exception("El espacio físico ya está ocupado por otro curso en este horario.");
                    }
                }

                // 3. Ejecutar el UPDATE si no hay choques
                string query = @"UPDATE Curso 
                         SET Nombre=@Nombre, Categoria=@Categoria, CupoMaximo=@CupoMaximo, 
                             InstructorID=@InstructorID, EspacioID=@EspacioID, HorarioID=@HorarioID 
                         WHERE CursoID=@CursoID";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CursoID", curso.CursoID);
                    cmd.Parameters.AddWithValue("@Nombre", curso.Nombre);
                    cmd.Parameters.AddWithValue("@Categoria", curso.Categoria);
                    cmd.Parameters.AddWithValue("@CupoMaximo", curso.CupoMaximo);
                    cmd.Parameters.AddWithValue("@InstructorID", curso.InstructorID);
                    cmd.Parameters.AddWithValue("@EspacioID", curso.EspacioID);
                    cmd.Parameters.AddWithValue("@HorarioID", curso.HorarioID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Método para eliminar un curso (DELETE)
        public void Eliminar(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_cadenaConexion))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Curso WHERE CursoID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
        // Aquí irían los métodos ObtenerPorId, Actualizar (PUT) y Eliminar (DELETE) siguiendo esta misma lógica...
    }
}
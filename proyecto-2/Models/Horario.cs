using System;

namespace proyecto_2.Models
{
    public class Horario
    {
        public int HorarioID { get; set; }
        public string DiaSemana { get; set; } 
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
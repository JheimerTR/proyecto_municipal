using System;

namespace proyecto_2.Models
{
    public class Inscripcion
    {
        public int InscripcionID { get; set; }
        public int ParticipanteID { get; set; }
        public int CursoID { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estado { get; set; }

    }
}
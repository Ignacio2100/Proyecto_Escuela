using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class TestCLS
    {
        public TestCLS()
        {
            Preguntas = new List<Pregunta>();  // Inicializar la lista para evitar el NullReferenceException
        }

        public string NombreEstudiante { get; set; }
        public List<Pregunta> Preguntas { get; set; }
    }

    public class Pregunta
    {
        public int Id { get; set; }
        public string Texto { get; set; } // Mantenemos el texto para uso interno
        public List<Opcion> Opciones { get; set; }
        public int RespuestaCorrectaId { get; set; }
        public int? RespuestaSeleccionadaId { get; set; }
        public string ImagenUrl { get; set; } // Imagen de la pregunta
    }

    public class Opcion
    {
        public int Id { get; set; }
        public string ImagenUrl { get; set; }
    }
}
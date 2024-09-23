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
        public string Texto { get; set; }
        public List<string> Opciones { get; set; }
        public string RespuestaCorrecta { get; set; }
        public string RespuestaSeleccionada { get; set; }
    }
}
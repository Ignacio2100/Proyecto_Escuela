using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class AnimalCLS
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string TipoAnimal { get; set; }
        public string ImagenUrl { get; set; }
    }
}
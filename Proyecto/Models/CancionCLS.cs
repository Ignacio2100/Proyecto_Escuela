using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class CancionCLS
    {

        [Display(Name = "Id Cancion")]
        public int ID_Cancion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(250, ErrorMessage = "La longitud maxima es 250")]
        [Display(Name = "Nombre de la Canción")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripción es obligatorio.")]
        [StringLength(750, ErrorMessage = "La longitud maxima es 750")]
        [Display(Name = "Descripcion de la Canción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Link es obligatorio.")]
        [Display(Name = "Link de la Canción")]
        public string Link { get; set; }

        [Display(Name = "Imagen de la Canción")]
        public string ImagenUrl { get; set; }
    }
}
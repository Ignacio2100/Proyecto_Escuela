using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class AnimalCLS
    {
        [Display (Name = "Id Animal")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength (250, ErrorMessage ="La longitud maxima es 250")]
        [Display (Name ="Nombre del Animal")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Caracteristicas del Animal es obligatorio.")]
        [StringLength(750, ErrorMessage = "La longitud maxima es 750")]
        [Display(Name ="Caracteristicas Puntuales del Animal")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La Clasificacion del Animal es obligatorio.")]
        [Display (Name ="Clasificacion del Animal")]
        public string TipoAnimal { get; set; }

        [Display (Name ="Imagen del Animal")]
        public string ImagenUrl { get; set; }
    }
}
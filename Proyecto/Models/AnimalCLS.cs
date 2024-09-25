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
        [StringLength (250, ErrorMessage ="La longitud máxima es 250")]
        [Display (Name ="Nombre del Animal")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Características del Animal es obligatorio.")]
        [StringLength(750, ErrorMessage = "La longitud máxima es 750")]
        [Display(Name ="Características Puntuales del Animal")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La Clasificación del Animal es obligatorio.")]
        [Display (Name ="Clasificación del Animal")]
        public string TipoAnimal { get; set; }

        [Display (Name ="Imagen del Animal")]
        public string ImagenUrl { get; set; }
    }
}
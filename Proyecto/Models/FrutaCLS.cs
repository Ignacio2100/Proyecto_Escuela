using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class FrutaCLS
    {
        [Display(Name = "Id Fruta")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(250, ErrorMessage = "La longitud máxima es 250")]
        [Display(Name = "Nombre del elemento")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La características del elemento es obligatorio.")]
        [StringLength(750, ErrorMessage = "La longitud máxima es 750")]
        [Display(Name = "Características Puntuales del Elemento")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La clasificación del elemento es obligatorio.")]
        [Display(Name = "Clasificación del elemento")]
        public string TipoElemento{ get; set; }

        [Display(Name = "Imagen del elemento")]
        public string ImagenUrl { get; set; }
    }
}
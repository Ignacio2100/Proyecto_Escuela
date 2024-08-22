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
        [StringLength(250, ErrorMessage = "La longitud maxima es 250")]
        [Display(Name = "Nombre del Elemento")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Caracteristicas del Elemento es obligatorio.")]
        [StringLength(750, ErrorMessage = "La longitud maxima es 750")]
        [Display(Name = "Caracteristicas Puntuales del Elemento")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La Clasificacion del Elemento es obligatorio.")]
        [Display(Name = "Clasificacion del Elemento")]
        public string TipoElemento{ get; set; }

        [Display(Name = "Imagen del Elemento")]
        public string ImagenUrl { get; set; }
    }
}
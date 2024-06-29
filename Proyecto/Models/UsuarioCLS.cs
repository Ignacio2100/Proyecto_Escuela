using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class UsuarioCLS
    {
        public int iidusuario { get; set; }

        [Required]
        public string nombreusuario { get; set; }

        [Required]
        public string contra { get; set; }

        public string iidtipousuario { get; set; }
        [Required]
        public int iid { get; set; }

    }
}
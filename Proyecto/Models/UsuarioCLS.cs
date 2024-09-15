using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class UsuarioCLS
    {
        public int USUARIO_ID { get; set; }

       
        public string USUARIO_CODIGO { get; set; }

        [Required]
        [EmailAddress]
        public string USUARIO_EMAIL { get; set; }

      
        public string USUARIO_PASSWORD { get; set; }

       
        public int ROL_ID { get; set; }

        public string ROL_NOMBRE { get; set; }

        public bool ESTADO { get; set; }

        public bool CAMBIO_PASSWORD { get; set; }

        public int INTENTOS_AUTENTICACION { get; set; }

    }
}
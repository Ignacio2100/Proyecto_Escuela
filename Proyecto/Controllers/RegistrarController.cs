using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class RegistrarController : Controller
    {
        // GET: Registrar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(UsuarioCLS usuarioCLS)
        {
            try
            {
                using (var db = new ESCUELAEntities())
                {
                    // Verificar si el usuario ya existe
                    if (db.USUARIO.Any(u => u.USUARIO_CODIGO == usuarioCLS.USUARIO_CODIGO || u.USUARIO_EMAIL == usuarioCLS.USUARIO_EMAIL))
                    {
                        return Json(new { success = false, message = "El código de usuario o email ya está en uso." });
                    }


                    // Crear nuevo usuario
                    var nuevoUsuario = new USUARIO
                    {
                        USUARIO_CODIGO = usuarioCLS.USUARIO_CODIGO,
                        USUARIO_EMAIL = usuarioCLS.USUARIO_EMAIL,
                        USUARIO_PASSWORD = CifrarContraseña(usuarioCLS.USUARIO_PASSWORD),
                        ROL_ID = 2, // Asumiendo que 2 es el ID para el rol "estudiante"
                        ESTADO = true,
                        CAMBIO_PASSWORD = false,
                        INTENTOS_AUTENTICACION = 0
                    };

                    db.USUARIO.Add(nuevoUsuario);
                    db.SaveChanges();

                    // Enviar correo de bienvenida
                    bool correoEnviado = EnviarCorreoBienvenida(usuarioCLS.USUARIO_EMAIL, usuarioCLS.USUARIO_CODIGO);


                    if (correoEnviado)
                    {
                        return Json(new { success = true, message = "Usuario registrado exitosamente y correo de bienvenida enviado" });
                    }
                    else
                    {
                        return Json(new { success = true, message = "Usuario registrado exitosamente, pero hubo un problema al enviar el correo de bienvenida" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        private string CifrarContraseña(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Método para enviar correo de bienvenida
        private bool EnviarCorreoBienvenida(string email, string codigoUsuario)
        {
            try
            {
                // Configura el cliente SMTP
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ignaciodeleone8@gmail.com", "fgpv ufwi wvmj jvko"), // Cambia la contraseña a una específica para aplicaciones si es necesario
                    EnableSsl = true,
                };

                // Crea el mensaje de correo
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ignaciodeleone8@gmail.com"),
                    Subject = "¡Bienvenido a la Página Interactiva de la Escuelita Club de Leones!",
                    Body = @"
    <html>
    <head>
        <style>
            body {
                font-family: Arial, sans-serif;
                color: #333;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }
            .container {
                width: 80%;
                margin: 0 auto;
                background-color: #fff;
                padding: 20px;
                border-radius: 8px;
                box-shadow: 0 0 10px rgba(0,0,0,0.1);
            }
            .header {
                background-color: #4CAF50;
                color: #fff;
                padding: 10px;
                text-align: center;
                border-radius: 8px 8px 0 0;
            }
            .content {
                padding: 20px;
                text-align: center;
            }
            .footer {
                background-color: #eee;
                padding: 10px;
                text-align: center;
                border-radius: 0 0 8px 8px;
                font-size: 12px;
                color: #777;
            }
            a {
                color: #4CAF50;
                text-decoration: none;
            }
            a:hover {
                text-decoration: underline;
            }
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>¡Bienvenido a la Escuelita Club de Leones!</h1>
            </div>
            <div class='content'>
                <p>Hola Amig@</p>
                <p>¡Gracias por registrarte en nuestra plataforma!</p>
                <p>Estamos encantados de tenerte con nosotros. En la Escuelita Club de Leones, tu aprendizaje y diversión están garantizados.</p>
                <p>No dudes en explorar todas las herramientas y recursos que hemos preparado para ti.</p>
                <p>Si tienes alguna pregunta, no dudes en ponerte en contacto con nosotros</p>
                <p>¡Bienvenido a bordo!</p>
                <p>Atentamente,<br>El equipo de la Escuelita Club de Leones</p>
            </div>
            <div class='footer'>
                <p>&copy; 2024 Escuelita Club de Leones. Todos los derechos reservados.</p>
                <p><a href='http://www.clubdeleones.com'>Visita nuestro sitio web</a></p>
            </div>
        </div>
    </body>
    </html>",
                    IsBodyHtml = true // Indica que el cuerpo del correo es HTML
                };

                mailMessage.To.Add(email);

                // Envía el correo
                smtpClient.Send(mailMessage);
                return true; // Envío exitoso
            }
            catch (SmtpException smtpEx)
            {
                // Maneja excepciones específicas del SMTP
                Console.WriteLine("SMTP Error: " + smtpEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Maneja cualquier otra excepción
                Console.WriteLine("Error general al enviar el correo: " + ex.Message);
                return false;
            }
        }

    }
}
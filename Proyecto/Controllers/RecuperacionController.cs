using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class RecuperacionController : Controller
    {
        // GET: Recuperacion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recuperar(string USUARIO_CODIGO, string USUARIO_EMAIL)
        {
            try
            {
                using (var bd = new ESCUELAEntities())
                {
                    var usuario = bd.USUARIO.FirstOrDefault(u => u.USUARIO_CODIGO == USUARIO_CODIGO && u.USUARIO_EMAIL == USUARIO_EMAIL);

                    if (usuario != null)
                    {
                        // Generar nueva contraseña
                        string nuevaContraseña = GenerarContraseñaAleatoria();

                        // Actualizar la contraseña en la base de datos (encriptada)
                        usuario.USUARIO_PASSWORD = EncriptarContraseña(nuevaContraseña);
                        usuario.CAMBIO_PASSWORD = true;
                        bd.SaveChanges();

                        // Enviar correo con la nueva contraseña
                        EnviarCorreo(usuario.USUARIO_EMAIL, nuevaContraseña);

                        return Json(new { success = true, message = "Se ha enviado una nueva contraseña a su correo electrónico." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "No se encontró un usuario con esas credenciales." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la solicitud: " + ex.Message });
            }
        }

        private string GenerarContraseñaAleatoria()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string EncriptarContraseña(string contraseña)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void EnviarCorreo(string email, string nuevaContraseña)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("ignaciodeleone8@gmail.com", "fgpv ufwi wvmj jvko");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ignaciodeleone8@gmail.com"),
                    Subject = "Recuperación de contraseña",
                    Body = $"Su nueva contraseña es: {nuevaContraseña}. Por favor, Cuidala.",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);

                client.Send(mailMessage);
            }
        }
    }
}
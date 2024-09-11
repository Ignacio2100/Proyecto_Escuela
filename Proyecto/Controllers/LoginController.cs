using Proyecto.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Data.Entity;

namespace Proyecto.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Login(UsuarioCLS oUsuario)
        {
            string mensaje = "";

            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                mensaje += "<ul class='list-group'>";
                foreach (var error in query)
                {
                    mensaje += "<li class='list-group-item'>" + error + "</li>";
                }
                mensaje += "</ul>";
            }
            else
            {
                string email = oUsuario.USUARIO_EMAIL;
                string password = oUsuario.USUARIO_PASSWORD;

                // Cifrar contraseña
                SHA256Managed sha = new SHA256Managed();
                byte[] byteContra = Encoding.UTF8.GetBytes(password);
                byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");

                using (var bd = new ESCUELAEntities())
                {
                    var usuario = bd.USUARIO.Include(u => u.ROL).FirstOrDefault(u => u.USUARIO_EMAIL == email && u.USUARIO_PASSWORD == cadenaContraCifrada && u.ESTADO == true);

                    if (usuario != null)
                    {
                        // Login exitoso
                        Session["Usuario"] = usuario.USUARIO_EMAIL;
                        Session["Rol"] = usuario.ROL.ROL_NOMBRE; // Guardar el rol en sesión
                        mensaje = "Login exitoso";

                        // Restablecer intentos de autenticación
                        usuario.INTENTOS_AUTENTICACION = 0;
                        bd.SaveChanges();
                    }
                    else
                    {
                        // Login fallido
                        mensaje = "Usuario o Contraseña incorrecta";
                        var usuarioParaActualizar = bd.USUARIO.FirstOrDefault(u => u.USUARIO_EMAIL == email);
                        if (usuarioParaActualizar != null)
                        {
                            // Incrementar intentos de autenticación
                            usuarioParaActualizar.INTENTOS_AUTENTICACION++;

                            // Verificar si se debe bloquear el usuario
                            if (usuarioParaActualizar.INTENTOS_AUTENTICACION >= 5)
                            {
                                usuarioParaActualizar.ESTADO = false;
                                mensaje = "Usuario bloqueado después de múltiples intentos fallidos";
                            }

                            bd.SaveChanges();
                        }
                    }
                }
            }

            return mensaje;
        }
    }
}
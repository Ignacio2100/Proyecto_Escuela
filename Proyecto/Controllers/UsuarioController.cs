using Proyecto.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Proyecto.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            using (var db = new ESCUELAEntities())
            {
                var usuarios = db.USUARIO.Where(u => u.ESTADO == true)
                    .Select(u => new UsuarioCLS
                    {
                        USUARIO_ID = u.USUARIO_ID,
                        USUARIO_CODIGO = u.USUARIO_CODIGO,
                        USUARIO_EMAIL = u.USUARIO_EMAIL,
                        ESTADO = u.ESTADO ?? false,
                        ROL_ID = (int)u.ROL_ID,
                        ROL_NOMBRE = u.ROL.ROL_NOMBRE //  relación entre USUARIO y ROL
                    }).ToList();

                ViewBag.Roles = db.ROL.Where(r => r.ESTADO == true)
                    .Select(r => new SelectListItem
                    {
                        Value = r.ROL_ID.ToString(),
                        Text = r.ROL_NOMBRE
                    }).ToList();

                return View(usuarios);
            }
        }

        // GET: Usuario/Registro
        public ActionResult Registro()
        {
            return View();
        }

        // POST: Usuario/Registro
        [HttpPost]
        public ActionResult Registro(UsuarioCLS modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new ESCUELAEntities())
                    {
                        // Verificar si el email ya está registrado
                        if (db.USUARIO.Any(u => u.USUARIO_EMAIL == modelo.USUARIO_EMAIL))
                        {
                            return Json(new { success = false, message = "Este correo electrónico ya está registrado." });
                        }

                        // Crear nuevo usuario
                        var nuevoUsuario = new USUARIO
                        {
                            USUARIO_CODIGO = modelo.USUARIO_CODIGO,
                            USUARIO_EMAIL = modelo.USUARIO_EMAIL,
                            USUARIO_PASSWORD = CifrarContraseña(modelo.USUARIO_PASSWORD),
                            ROL_ID = modelo.ROL_ID,
                            ESTADO = true,
                            CAMBIO_PASSWORD = false,
                            INTENTOS_AUTENTICACION = 0
                        };

                        db.USUARIO.Add(nuevoUsuario);
                        db.SaveChanges();

                        return Json(new { success = true, message = "Usuario registrado con éxito." });
                    }
                }
                catch (Exception ex)
                {
                    // Captura la excepción completa, incluyendo la inner exception
                    return Json(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
                }
            }

            return Json(new { success = false, message = "Datos inválidos." });
        }

        // POST: Usuario/Eliminar
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                using (var db = new ESCUELAEntities())
                {
                    var usuario = db.USUARIO.Find(id);
                    if (usuario != null)
                    {
                        usuario.ESTADO = false;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Usuario eliminado con éxito." });
                    }
                    return Json(new { success = false, message = "Usuario no encontrado." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
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



        // GET: Usuario/Editar/5
        public ActionResult Editar(int id)
        {
            using (var db = new ESCUELAEntities())
            {
                var usuario = db.USUARIO.Find(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                var usuarioCLS = new UsuarioCLS
                {
                    USUARIO_ID = usuario.USUARIO_ID,
                    USUARIO_CODIGO = usuario.USUARIO_CODIGO,
                    USUARIO_EMAIL = usuario.USUARIO_EMAIL,
                    ROL_ID = (int)usuario.ROL_ID,
                    ESTADO = usuario.ESTADO ?? false
                };

                ViewBag.Roles = db.ROL.Where(r => r.ESTADO == true)
                    .Select(r => new SelectListItem
                    {
                        Value = r.ROL_ID.ToString(),
                        Text = r.ROL_NOMBRE,
                        Selected = r.ROL_ID == usuario.ROL_ID
                    }).ToList();

                return View(usuarioCLS);
            }
        }

        // POST: Usuario/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UsuarioCLS modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new ESCUELAEntities())
                    {
                        var usuario = db.USUARIO.Find(modelo.USUARIO_ID);
                        if (usuario == null)
                        {
                            return Json(new { success = false, message = "Usuario no encontrado" });
                        }

                        usuario.USUARIO_CODIGO = modelo.USUARIO_CODIGO;
                        usuario.USUARIO_EMAIL = modelo.USUARIO_EMAIL;
                        usuario.ROL_ID = modelo.ROL_ID;
                        usuario.ESTADO = modelo.ESTADO;

                        if (!string.IsNullOrEmpty(modelo.USUARIO_PASSWORD))
                        {
                            usuario.USUARIO_PASSWORD = CifrarContraseña(modelo.USUARIO_PASSWORD);
                            usuario.CAMBIO_PASSWORD = true;
                        }

                        db.SaveChanges();
                        return Json(new { success = true, message = "Usuario actualizado con éxito" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar: " + ex.Message });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos", errors = errors });
        }








    }
}
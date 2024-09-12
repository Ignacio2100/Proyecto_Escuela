using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class RolController : Controller
    {
        [Authorize]
        // GET: Rol
        public ActionResult Index()
        {
            using (var db = new ESCUELAEntities())
            {
                var roles = db.ROL.Where(r => r.ESTADO == true).Select(r => new RolCLS
                {
                    ROL_ID = r.ROL_ID,
                    ROL_NOMBRE = r.ROL_NOMBRE,
                    ESTADO = r.ESTADO ?? false
                }).ToList();
                return View(roles);
            }
        }


        // GET: Rol/Registro
        public ActionResult Registro()
        {
            return View();
        }

        // POST: Rol/Registro
        [HttpPost]
        public ActionResult Registro(RolCLS modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new ESCUELAEntities())
                    {
                        // Verificar si el ROL ya está registrado
                        if (db.ROL.Any(u => u.ROL_NOMBRE == modelo.ROL_NOMBRE))
                        {
                            return Json(new { success = false, message = "Este Rol ya está registrado." });
                        }


                        // Crear nuevo usuario
                        var nuevoRol = new ROL
                        {
                          
                            ROL_NOMBRE = modelo.ROL_NOMBRE,
                            ESTADO = true,
                        
                        };

                        db.ROL.Add(nuevoRol);
                        db.SaveChanges();

                        return Json(new { success = true, message = "Rol registrado con éxito." });
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

        // POST: Rol/Eliminar
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                using (var db = new ESCUELAEntities())
                {
                    var rol = db.ROL.Find(id);
                    if (rol != null)
                    {
                        rol.ESTADO = false;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Rol eliminado con éxito." });
                    }
                    return Json(new { success = false, message = "Rol no encontrado." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }


}

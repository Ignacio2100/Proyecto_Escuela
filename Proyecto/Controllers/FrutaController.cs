using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Proyecto.Controllers.FrutaController;

namespace Proyecto.Controllers
{
    public class FrutaController : Controller
    {
       
        public class ElementoViewModel
        {
            public List<FrutaCLS> ListaFrutas { get; set; }
            public List<FrutaCLS> ListaVerduras { get; set; }
            public FrutaCLS NuevoElemento { get; set; }
        }
        [Authorize]
        // GET: Fruta
        public ActionResult Index()
        {
            ElementoViewModel viewModel = new ElementoViewModel();
            viewModel.NuevoElemento = new FrutaCLS();
            try
            {
                using (var bd = new ESCUELAEntities())
            {
                viewModel.ListaFrutas = (from Frutas in bd.Frutas
                                         where Frutas.TipoElemento == "Fruta"
                                         select new FrutaCLS
                                         {
                                             Id = Frutas.ID_Fruta,
                                             Nombre = Frutas.Nombre,
                                             Descripcion = Frutas.Descripcion,
                                             ImagenUrl = Frutas.Imagen
                                         }).ToList();

                viewModel.ListaVerduras = (from Frutas in bd.Frutas
                                           where Frutas.TipoElemento == "Verdura"
                                           select new FrutaCLS
                                           {
                                               Id = Frutas.ID_Fruta,
                                               Nombre = Frutas.Nombre,
                                               Descripcion = Frutas.Descripcion,
                                               ImagenUrl = Frutas.Imagen
                                           }).ToList();
            }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                // Log o manejar el error de conexión aquí
                // Ejemplo: Guardar en el log el mensaje de error
                // Logger.LogError(ex.Message);

                // Puedes asignar una lista vacía para que la vista no falle
                viewModel.ListaVerduras = new List<FrutaCLS>();
                viewModel.ListaFrutas = new List<FrutaCLS>();

                // Opcional: Mostrar un mensaje de error en la vista
                ViewBag.ErrorMessage = "No se pudo conectar a la base de datos. Mostrando datos vacíos.";
            }
            return View(viewModel);
        }

        // GET: Frutas/Agregar
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ElementoViewModel viewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                using (var bd = new ESCUELAEntities())
                {
                    Frutas oFrutas = new Frutas();
                    oFrutas.Nombre = viewModel.NuevoElemento.Nombre;
                    oFrutas.TipoElemento = viewModel.NuevoElemento.TipoElemento;
                    oFrutas.Descripcion = viewModel.NuevoElemento.Descripcion;

                    // Manejo de la imagen
                    if (upload != null && upload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        string folderPath = Server.MapPath("~/Content/Imagenes");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        string path = Path.Combine(folderPath, fileName);
                        upload.SaveAs(path);
                        oFrutas.Imagen = "~/Content/Imagenes/" + fileName;
                    }

                    bd.Frutas.Add(oFrutas);
                    bd.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, vuelve a la vista con el modelo
            return View(viewModel);
        }
    }
}
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

        // GET: Fruta
        public ActionResult Index()
        {
            ElementoViewModel viewModel = new ElementoViewModel();
            viewModel.NuevoElemento = new FrutaCLS();

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
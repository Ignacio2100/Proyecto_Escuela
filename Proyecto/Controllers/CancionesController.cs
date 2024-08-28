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
    public class CancionesController : Controller
    {
        public class CancionViewModel
        {
            public CancionCLS Cancion { get; set; }
            public List<CancionCLS> ListaCanciones { get; set; }
        }


        // GET: Canciones
        public ActionResult Index()
        {
            CancionViewModel viewModel = new CancionViewModel();

            using (var bd = new ESCUELAEntities())
            {
                viewModel.ListaCanciones = (from Canciones in bd.Canciones
                                            select new CancionCLS
                                            {
                                                ID_Cancion = Canciones.ID_Cancion,
                                                Nombre = Canciones.Nombre,
                                                Descripcion = Canciones.Descripcion,
                                                Link = Canciones.Link,
                                                ImagenUrl = Canciones.Imagen,
                                            }).ToList();
            }
            return View(viewModel);
        }

        // GET: Canciones/Agregar
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(CancionViewModel viewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                using (var bd = new ESCUELAEntities())
                {
                    Canciones oCanciones = new Canciones();
                    oCanciones.Nombre = viewModel.Cancion.Nombre;
                    oCanciones.Link = viewModel.Cancion.Link;
                    oCanciones.Descripcion = viewModel.Cancion.Descripcion;

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
                        oCanciones.Imagen = "~/Content/Imagenes/" + fileName;
                    }

                    bd.Canciones.Add(oCanciones);
                    bd.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, vuelve a la vista con el modelo
            return View(viewModel);
        }
    }
}
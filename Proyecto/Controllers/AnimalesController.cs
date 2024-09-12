using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
   
    public class AnimalesController : Controller
    {
        [Authorize]
        public class AnimalesViewModel
        {
            public List<AnimalCLS> ListaMamiferos { get; set; }
            public List<AnimalCLS> ListaAves { get; set; }
            public List<AnimalCLS> ListaReptiles { get; set; }
            public List<AnimalCLS> ListaAnfibios { get; set; }
            public List<AnimalCLS> ListaVertebrados { get; set; }
            public List<AnimalCLS> ListaInvertebrados { get; set; }
            public AnimalCLS NuevoAnimal { get; set; }
        }
        // GET: Animales
        public ActionResult Index()
        {
            AnimalesViewModel viewModel = new AnimalesViewModel();
            viewModel.NuevoAnimal = new AnimalCLS();

            try
            {
                using (var bd = new ESCUELAEntities())
            {
                viewModel.ListaMamiferos = (from Animales in bd.Animales
                                            where Animales.TipoAnimal == "Mamiferos"
                                            select new AnimalCLS
                                            {
                                                Id = Animales.ID_Animal,
                                                Nombre = Animales.Nombre,
                                                Descripcion = Animales.Descripcion,
                                                ImagenUrl = Animales.Imagen
                                            }).ToList();

                viewModel.ListaAves = (from Animales in bd.Animales
                                       where Animales.TipoAnimal == "Aves"
                                       select new AnimalCLS
                                       {
                                           Id = Animales.ID_Animal,
                                           Nombre = Animales.Nombre,
                                           Descripcion = Animales.Descripcion,
                                           ImagenUrl = Animales.Imagen
                                       }).ToList();

                viewModel.ListaReptiles = (from Animales in bd.Animales
                                           where Animales.TipoAnimal == "Reptiles"
                                           select new AnimalCLS
                                           {
                                               Id = Animales.ID_Animal,
                                               Nombre = Animales.Nombre,
                                               Descripcion = Animales.Descripcion,
                                               ImagenUrl = Animales.Imagen
                                           }).ToList();

                viewModel.ListaAnfibios = (from Animales in bd.Animales
                                           where Animales.TipoAnimal == "Anfibios"
                                           select new AnimalCLS
                                           {
                                               Id = Animales.ID_Animal,
                                               Nombre = Animales.Nombre,
                                               Descripcion = Animales.Descripcion,
                                               ImagenUrl = Animales.Imagen
                                           }).ToList();

                viewModel.ListaVertebrados = (from Animales in bd.Animales
                                              where Animales.TipoAnimal == "Vertebrados"
                                              select new AnimalCLS
                                              {
                                                  Id = Animales.ID_Animal,
                                                  Nombre = Animales.Nombre,
                                                  Descripcion = Animales.Descripcion,
                                                  ImagenUrl = Animales.Imagen
                                              }).ToList();

                viewModel.ListaInvertebrados = (from Animales in bd.Animales
                                                where Animales.TipoAnimal == "Invertebrados"
                                                select new AnimalCLS
                                                {
                                                    Id = Animales.ID_Animal,
                                                    Nombre = Animales.Nombre,
                                                    Descripcion = Animales.Descripcion,
                                                    ImagenUrl = Animales.Imagen
                                                }).ToList();

            }
            }
            catch (System.Data.Entity.Core.EntityException )
            {
                // Log o manejar el error de conexión aquí
                // Ejemplo: Guardar en el log el mensaje de error
                // Logger.LogError(ex.Message);

                // Puedes asignar una lista vacía para que la vista no falle
                viewModel.ListaMamiferos = new List<AnimalCLS>();
                viewModel.ListaAves = new List<AnimalCLS>();
                viewModel.ListaReptiles = new List<AnimalCLS>();
                viewModel.ListaAnfibios = new List<AnimalCLS>();
                viewModel.ListaVertebrados = new List<AnimalCLS>();
                viewModel.ListaInvertebrados = new List<AnimalCLS>();


                // Opcional: Mostrar un mensaje de error en la vista
                ViewBag.ErrorMessage = "No se pudo conectar a la base de datos. Mostrando datos vacíos.";
            }
            return View(viewModel);

        }

        // GET: Animales/Agregar
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(AnimalCLS nuevoAnimal, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                using (var bd = new ESCUELAEntities())
                {
                    Animales oAnimales = new Animales();
                    oAnimales.Nombre = nuevoAnimal.Nombre;
                    oAnimales.TipoAnimal = nuevoAnimal.TipoAnimal;
                    oAnimales.Descripcion = nuevoAnimal.Descripcion;

                    // Manejo de la imagen
                    if (upload != null && upload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        string folderPath = Server.MapPath("~/Content/Imagenes");

                        // Verificar si la carpeta existe, si no, crearla
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string path = Path.Combine(folderPath, fileName);
                        upload.SaveAs(path);
                        oAnimales.Imagen = "~/Content/Imagenes/" + fileName;
                    }


                    bd.Animales.Add(oAnimales);
                    bd.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, vuelve a la vista con el modelo
            var viewModel = new AnimalesViewModel { NuevoAnimal = nuevoAnimal };
            return View("Index", viewModel);
        }


    }
}

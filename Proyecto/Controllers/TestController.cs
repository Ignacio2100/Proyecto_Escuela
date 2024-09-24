using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class TestController : Controller
    {
        private static List<Pregunta> preguntas = new List<Pregunta>
        {
           new Pregunta { Id = 1, Texto = "¿Cuál es el color rojo?", Opciones = new List<string> { "Rojo", "Negro", "Azul" }, RespuestaCorrecta = "Rojo", ImagenUrl = "Imagenes/Rojo.png" },
           new Pregunta { Id = 2, Texto = "¿Cuál es la primera letra del abecedario?", Opciones = new List<string> { "A", "B", "C" }, RespuestaCorrecta = "A", ImagenUrl = "/images/a.png" },
           new Pregunta { Id = 3, Texto = "¿Qué tipo de fruta es una manzana?", Opciones = new List<string> { "Fruta", "Verdura", "Carne" }, RespuestaCorrecta = "Fruta", ImagenUrl = "/images/fruta.png" },
           new Pregunta { Id = 4, Texto = "¿Qué tipo de animal es un perro?", Opciones = new List<string> { "Mamífero", "Ave", "Reptil" }, RespuestaCorrecta = "Mamífero", ImagenUrl = "/images/mamifero.png" },
           new Pregunta { Id = 5, Texto = "¿Cuál es una vocal?", Opciones = new List<string> { "E", "T", "R" }, RespuestaCorrecta = "E", ImagenUrl = "/images/e.png" },
           new Pregunta { Id = 6, Texto = "¿Qué figura es un triángulo?", Opciones = new List<string> { "Triángulo", "Círculo", "Cuadrado" }, RespuestaCorrecta = "Triángulo", ImagenUrl = "/images/triangulo.png" }
};

        public ActionResult Index()
        {
            var model = new TestCLS
            {
                NombreEstudiante = string.Empty, // Inicializar el nombre como vacío
                Preguntas = preguntas // Cargar las preguntas directamente
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EnviarRespuestas(TestCLS model)
        {
            if (ModelState.IsValid && model.Preguntas != null && model.Preguntas.Count > 0)
            {
                try
                {
                    using (var db = new ESCUELAEntities())
                    {
                        foreach (var pregunta in model.Preguntas)
                        {
                            var respuestaCorrecta = preguntas.First(p => p.Id == pregunta.Id).RespuestaCorrecta;
                            var esCorrecta = (pregunta.RespuestaSeleccionada == respuestaCorrecta) ? 1 : 0; // Guardar 1 o 0

                            var resultado = new ResultadosTest
                            {
                                NombreEstudiante = model.NombreEstudiante,
                                Pregunta = pregunta.Texto,
                                Respuesta = pregunta.RespuestaSeleccionada,
                                EsCorrecta = (pregunta.RespuestaSeleccionada == respuestaCorrecta)
                            };

                            db.ResultadosTest.Add(resultado);
                        }
                        db.SaveChanges();
                    }
                   
                    return RedirectToAction("FinalizarTest", new { nombreEstudiante = model.NombreEstudiante });

                   
                }
                catch (Exception)
                {
                    ViewBag.Error = "Hubo un problema al guardar las respuestas. Intenta nuevamente.";
                    return View("Test", model);
                }
            }
            else
            {
                ViewBag.Error = "El modelo no es válido o faltan preguntas.";
                return View("Test", model);
            }
        }

        public ActionResult FinalizarTest(string nombreEstudiante)
        {
            try
            {
                using (var db = new ESCUELAEntities())
                {
                    var resultados = db.ResultadosTest.Where(r => r.NombreEstudiante == nombreEstudiante).ToList();

                    // Generar PDF
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        document.Open();

                        document.Add(new Paragraph($"Resultados del Test de {nombreEstudiante}", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
                        document.Add(new Paragraph(" "));

                        PdfPTable table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        table.SetWidths(new int[] { 5, 3, 2 });

                        table.AddCell(new PdfPCell(new Phrase("Pregunta", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Respuesta", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Correcta", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));

                        foreach (var resultado in resultados)
                        {
                            table.AddCell(new PdfPCell(new Phrase(resultado.Pregunta)));
                            table.AddCell(new PdfPCell(new Phrase(resultado.Respuesta)));
                            table.AddCell(new PdfPCell(new Phrase(resultado.EsCorrecta ? "Sí" : "No")));
                        }

                        document.Add(table);
                        document.Close();

                        byte[] byteArray = ms.ToArray();



                        return File(byteArray, "application/pdf", $"Resultados_{nombreEstudiante}.pdf");

                       
                    }
                }
            }
            catch (Exception)
            {
                // Manejo de errores al generar el PDF.
                ViewBag.Error = "Hubo un problema al generar el PDF. Intenta nuevamente.";
                return View("Index");
            }
        }
    }
}

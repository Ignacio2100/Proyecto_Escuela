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
           new Pregunta {
               Id = 1,
               Texto = "¿Cuál es el color rojo?",
               Opciones = new List<Opcion> {
                   new Opcion { Id = 1, ImagenUrl = "/Imagenes/Rojo.png" },
                   new Opcion { Id = 2, ImagenUrl = "/Imagenes/Azul.png" },
                   new Opcion { Id = 3, ImagenUrl = "/Imagenes/Verde.png" }
               },
               RespuestaCorrectaId = 1,
               ImagenUrl = "/Imagenes/Pregunta1.png"
           },
           new Pregunta {
               Id = 2,
               Texto = "¿Cuál es la primera letra del abecedario?",
               Opciones = new List<Opcion> {
                   new Opcion { Id = 1, ImagenUrl = "/Imagenes/Ardilla.png" },
                   new Opcion { Id = 2, ImagenUrl = "/Imagenes/FresaJ.png" },
                   new Opcion { Id = 3, ImagenUrl = "/Imagenes/Uvas.png" }
               },
               RespuestaCorrectaId = 1,
               ImagenUrl = "/Imagenes/Pregunta2.png"
           },
           new Pregunta {
               Id = 3,
               Texto = "¿La Manzana es una verdura?",
               Opciones = new List<Opcion> {
                   new Opcion { Id = 1, ImagenUrl = "/Imagenes/NOSE.png" },
                   new Opcion { Id = 2, ImagenUrl = "/Imagenes/SII.png" },
                   new Opcion { Id = 3, ImagenUrl = "/Imagenes/NOO.png" }
               },
               RespuestaCorrectaId = 3,
               ImagenUrl = "/Imagenes/Pregunta3.png"
           },
           new Pregunta {
               Id = 4,
               Texto = "¿Qué tipo de animal es un perro?",
               Opciones = new List<Opcion> {
                   new Opcion { Id = 1, ImagenUrl = "/Imagenes/MamiferoP.png" },
                   new Opcion { Id = 2, ImagenUrl = "/Imagenes/AveP.png" },
                   new Opcion { Id = 3, ImagenUrl = "/Imagenes/ReptilP.png" }
               },
               RespuestaCorrectaId = 1,
               ImagenUrl = "/Imagenes/Pregunta4.png"
           },
           new Pregunta {
               Id = 5,
               Texto = "¿Cuál es una vocal?",
               Opciones = new List<Opcion> {
                   new Opcion { Id = 1, ImagenUrl = "/Imagenes/Ardilla.png" },
                   new Opcion { Id = 2, ImagenUrl = "/Imagenes/NOSE.png" },
                   new Opcion { Id = 3, ImagenUrl = "/Imagenes/CocoJ.png" }
               },
               RespuestaCorrectaId = 1,
               ImagenUrl = "/Imagenes/Pregunta5.png"
           },
            new Pregunta {
               Id = 6,
               Texto = "¿Cuál es un triángulo?",
               Opciones = new List<Opcion> {
                   new Opcion { Id = 1, ImagenUrl = "/Imagenes/Trapecio.png" },
                   new Opcion { Id = 2, ImagenUrl = "/Imagenes/Triangulo.png" },
                   new Opcion { Id = 3, ImagenUrl = "/Imagenes/Circulo.png" }
               },
               RespuestaCorrectaId = 2,
               ImagenUrl = "/Imagenes/Pregunta6.png"
           },
           // Añade más preguntas siguiendo este formato
        };

        [Authorize]
        public ActionResult Index()
        {
            var model = new TestCLS
            {
                NombreEstudiante = string.Empty,
                Preguntas = preguntas
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
                            var respuestaCorrecta = preguntas.First(p => p.Id == pregunta.Id).RespuestaCorrectaId;
                            var esCorrecta = (pregunta.RespuestaSeleccionadaId == respuestaCorrecta) ? 1 : 0;

                            var resultado = new ResultadosTest
                            {
                                NombreEstudiante = model.NombreEstudiante,
                                Pregunta = pregunta.Texto,
                                Respuesta = pregunta.RespuestaSeleccionadaId.ToString(),
                                EsCorrecta = (pregunta.RespuestaSeleccionadaId == respuestaCorrecta)
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
                    var resultados = db.ResultadosTest.Where(r => r.NombreEstudiante == nombreEstudiante)
                                                      .OrderByDescending(r => r.Id)  // Asumiendo que Id es auto-incremental
                                                      .ToList();

                    // Separar los resultados en recientes y anteriores
                    var resultadosRecientes = resultados.Take(6).ToList();
                    var resultadosAnteriores = resultados.Skip(6).ToList();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        document.Open();

                        // Agregar el logo
                        string logoPath = Server.MapPath("~/Imagenes/LOGO.PNG");
                        Image logo = Image.GetInstance(logoPath);
                        logo.ScaleToFit(100, 100);
                        logo.Alignment = Element.ALIGN_CENTER;
                        document.Add(logo);

                        // Título y información
                        Chunk titleChunk = new Chunk($"Resultados del Test de {nombreEstudiante}", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD, BaseColor.DARK_GRAY));
                        Paragraph title = new Paragraph(titleChunk);
                        title.Alignment = Element.ALIGN_CENTER;
                        title.SpacingBefore = 20f;
                        title.SpacingAfter = 10f;
                        document.Add(title);

                        Paragraph info = new Paragraph($"Fecha y hora de descarga: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}", new Font(Font.FontFamily.HELVETICA, 12, Font.ITALIC, BaseColor.GRAY));
                        info.Alignment = Element.ALIGN_CENTER;
                        info.SpacingAfter = 20f;
                        document.Add(info);

                        // Función para crear tabla de resultados
                        PdfPTable CreateResultTable(List<ResultadosTest> results, string tableTitle)
                        {
                            Paragraph tableTitlePara = new Paragraph(tableTitle, new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.DARK_GRAY));
                            tableTitlePara.SpacingBefore = 15f;
                            tableTitlePara.SpacingAfter = 10f;
                            document.Add(tableTitlePara);

                            PdfPTable table = new PdfPTable(3);
                            table.WidthPercentage = 100;
                            table.SetWidths(new float[] { 5f, 3f, 2f });

                            // Estilo para los encabezados
                            PdfPCell headerCell = new PdfPCell();
                            headerCell.BackgroundColor = new BaseColor(41, 128, 185); // Azul atractivo
                            headerCell.Padding = 5;

                            // Encabezados
                            headerCell.Phrase = new Phrase("Pregunta", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE));
                            table.AddCell(headerCell);
                            headerCell.Phrase = new Phrase("Selección de Respuesta", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE));
                            table.AddCell(headerCell);
                            headerCell.Phrase = new Phrase("Correcta", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE));
                            table.AddCell(headerCell);

                            // Estilo para las celdas de datos
                            PdfPCell dataCell = new PdfPCell();
                            dataCell.Padding = 5;

                            foreach (var resultado in results)
                            {
                                dataCell.Phrase = new Phrase(resultado.Pregunta, new Font(Font.FontFamily.HELVETICA, 10));
                                table.AddCell(dataCell);
                                dataCell.Phrase = new Phrase(resultado.Respuesta, new Font(Font.FontFamily.HELVETICA, 10));
                                table.AddCell(dataCell);
                                dataCell.Phrase = new Phrase(resultado.EsCorrecta ? "Sí" : "No", new Font(Font.FontFamily.HELVETICA, 10, resultado.EsCorrecta ? Font.NORMAL : Font.BOLD, resultado.EsCorrecta ? BaseColor.GREEN : BaseColor.RED));
                                table.AddCell(dataCell);
                            }

                            return table;
                        }

                        // Agregar tabla de resultados recientes
                        if (resultadosRecientes.Any())
                        {
                            document.Add(CreateResultTable(resultadosRecientes, "Resultados Recientes"));
                        }

                        // Agregar tabla de resultados anteriores
                        if (resultadosAnteriores.Any())
                        {
                            document.Add(CreateResultTable(resultadosAnteriores, "Resultados Anteriores"));
                        }

                        // Agregar pie de página
                        Paragraph footer = new Paragraph("Gracias por completar el test. ¡Sigue aprendiendo!", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC, BaseColor.GRAY));
                        footer.Alignment = Element.ALIGN_CENTER;
                        footer.SpacingBefore = 20f;
                        document.Add(footer);

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
                // Considera loggear la excepción: ex.Message
                return View("Index");
            }
        }
    }
}

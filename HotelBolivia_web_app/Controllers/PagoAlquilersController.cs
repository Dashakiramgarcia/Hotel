using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBolivia_web_app.Context;
using HotelBolivia_web_app.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;
using QuestPDF.Infrastructure;

namespace HotelBolivia_web_app.Controllers
{
    public class PagoAlquilersController : Controller
    {
        private readonly MiContext _context;

        public PagoAlquilersController(MiContext context)
        {
            _context = context;
        }

        // GET: PagoAlquilers
        public async Task<IActionResult> Index()
        {
            var miContext = _context.PagoAlquilers.Include(p => p.Usuario);
            return View(await miContext.ToListAsync());
        }

        // GET: PagoAlquilers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PagoAlquilers == null)
            {
                return NotFound();
            }

            var pagoAlquiler = await _context.PagoAlquilers
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagoAlquiler == null)
            {
                return NotFound();
            }

            return View(pagoAlquiler);
        }

        // GET: PagoAlquilers/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email");
            ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero");
            return View();
        }

        // POST: PagoAlquilers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ci,NombreCompleto,FechaRegistro,Dias,MontoTotal,NumFactura,HabtacionId,UsuarioId")] PagoAlquiler pagoAlquiler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pagoAlquiler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", pagoAlquiler.UsuarioId);
            return View(pagoAlquiler);
        }

        // GET: PagoAlquilers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PagoAlquilers == null)
            {
                return NotFound();
            }

            var pagoAlquiler = await _context.PagoAlquilers.FindAsync(id);
            if (pagoAlquiler == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", pagoAlquiler.UsuarioId);
            return View(pagoAlquiler);
        }

        // POST: PagoAlquilers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ci,NombreCompleto,FechaRegistro,Dias,MontoTotal,NumFactura,HabtacionId,UsuarioId")] PagoAlquiler pagoAlquiler)
        {
            if (id != pagoAlquiler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pagoAlquiler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoAlquilerExists(pagoAlquiler.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", pagoAlquiler.UsuarioId);
            return View(pagoAlquiler);
        }

        // GET: PagoAlquilers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PagoAlquilers == null)
            {
                return NotFound();
            }

            var pagoAlquiler = await _context.PagoAlquilers
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagoAlquiler == null)
            {
                return NotFound();
            }

            return View(pagoAlquiler);
        }

        // POST: PagoAlquilers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PagoAlquilers == null)
            {
                return Problem("Entity set 'MiContext.PagoAlquilers'  is null.");
            }
            var pagoAlquiler = await _context.PagoAlquilers.FindAsync(id);
            if (pagoAlquiler != null)
            {
                _context.PagoAlquilers.Remove(pagoAlquiler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoAlquilerExists(int id)
        {
          return (_context.PagoAlquilers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //aqui otro para la factura
        public IActionResult DescargarPDF()
        {

            var data = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(40);

                    page.Header().ShowOnce().Row(row =>
                    {
                        //var rutaImagen = Path.Combine(_context.WebRootPath, "./CRUD/wwwroot/img/logo.jpg");
                        //byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);
                        row.ConstantItem(140).Height(80).Placeholder();
                        //row.ConstantItem(150).Image(imageData);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("HOTEL BOLIVIA").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("").FontSize(9);
                            col.Item().AlignCenter().Text("SEM 2-2023").FontSize(9);
                            col.Item().AlignCenter().Text("Actualizacion - Tecnologica").FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272")
                            .AlignCenter().Text(" ENTREGA DE REPORTE");

                            col.Item().Background("#257272").Border(1)
                            .BorderColor("#257272").AlignCenter()
                            .Text("MENSUAL").FontColor("#fff");

                         


                        });
                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().Column(col2 =>
                        {
                            col2.Item().Text("DATOS DE LA RECEPCIOONISTA").Underline().Bold();

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Nombre: ").SemiBold().FontSize(10);
                                txt.Span("MARIA ANGELICA ROSALES FLORES").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("CI: ").SemiBold().FontSize(10);
                                txt.Span("7897897").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Direccion: ").SemiBold().FontSize(10);
                                txt.Span("AV. TACNA 123").FontSize(10);
                            });
                        });

                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().Table(async tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                             
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn(8);
                                columns.RelativeColumn(2);

                            });

                            tabla.Header(header =>
                            {
                                

                                header.Cell().Background("#257272")
                               .Padding(1).Text("CI").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(1).Text("NombreCompleto").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(1).Text("Fecha").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(1).Text("Dias").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(1).Text("Usuario").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(1).Text("NuFac").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(1).Text("Habitacion").FontColor("#fff");

                                header.Cell().Background("#257272")
                               .Padding(1).Text("Monto").FontColor("#fff");


                            });

                            /*
                             *  foreach (var item in Enumerable.Range(1, 45))
                             {
                                 var cantidad = Placeholders.Random.Next(1, 10);
                                 var precio = Placeholders.Random.Next(5, 15);
                                 var total = cantidad * precio;

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                 .Padding(2).Text(Placeholders.Label()).FontSize(10);

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                          .Padding(2).Text(cantidad.ToString()).FontSize(10);

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                          .Padding(2).Text($"S/. {precio}").FontSize(10);

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                          .Padding(2).AlignRight().Text($"S/. {total}").FontSize(10);
                             }
                             */

                            //*aqui
                            ;
                            //can = _context.Usuarios.Count();

                            foreach (var item in _context.PagoAlquilers)
                            {
                      
                                var ci = item.Ci.ToString();
                                var nomc = item.NombreCompleto.ToString();
                                var fecre= item.FechaRegistro.ToString();
                                var dia = item.Dias.ToString();
                                var idU = item.UsuarioId.ToString();
                              
                                var numfac = item.NumFactura.ToString();
                                var idH= item.HabtacionId.ToString();
                                var mont = item.MontoTotal.ToString();




                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(4).Text(ci.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(4).Text(nomc.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(4).Text(fecre.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(4).Text(dia.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(4).Text(idU.ToString()).FontSize(10);

                               

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(4).Text(numfac.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(4).Text(idH.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(4).Text(mont.ToString()).FontSize(10);


                            }

                            //*fin aqui


                        });

                        col1.Item().AlignRight().Text("Total: 639").FontSize(8);

                        if (1 == 1)
                            col1.Item().Background(Colors.Grey.Lighten3).Padding(8)
                            .Column(column =>
                            {
                                column.Item().Text("Comentarios").FontSize(14);
                                column.Item().Text(Placeholders.LoremIpsum());
                                column.Spacing(5);
                            });

                        col1.Spacing(8);
                    });


                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(data);
            return File(stream, "application/pdf", "detalleventa.pdf");

        }


    }
}
    


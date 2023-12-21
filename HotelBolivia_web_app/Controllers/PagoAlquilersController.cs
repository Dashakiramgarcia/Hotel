using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBolivia_web_app.Context;
using HotelBolivia_web_app.Models;

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
    }
}

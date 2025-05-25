using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HorseRace.Data;
using HorseRace.Models;

namespace HorseRace.Controllers
{
    public class KonieController : Controller
    {
        private readonly HorseRaceContext _context;

        public KonieController(HorseRaceContext context)
        {
            _context = context;
        }

        // GET: Konie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Konie.ToListAsync());
        }

        // GET: Konie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kon = await _context.Konie
                .Include(s => s.Wyscigi)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kon == null)
            {
                return NotFound();
            }

            return View(kon);
        }

        // GET: Konie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Konie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Umaszczenie,Wytrzymalosc,MaxWytrzymalosc,Szybkosc,MaxSzybkosc")] Kon kon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kon);
        }

        // GET: Konie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kon = await _context.Konie.FindAsync(id);
            if (kon == null)
            {
                return NotFound();
            }
            return View(kon);
        }

        // POST: Konie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Umaszczenie,Wytrzymalosc,MaxWytrzymalosc,Szybkosc,MaxSzybkosc")] Kon kon)
        {
            if (id != kon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonExists(kon.Id))
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
            return View(kon);
        }

        // GET: Konie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kon = await _context.Konie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kon == null)
            {
                return NotFound();
            }

            return View(kon);
        }

        // POST: Konie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kon = await _context.Konie.FindAsync(id);
            if (kon != null)
            {
                _context.Konie.Remove(kon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KonExists(int id)
        {
            return _context.Konie.Any(e => e.Id == id);
        }
    }
}

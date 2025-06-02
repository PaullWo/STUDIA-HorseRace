using HorseRace.Data;
using HorseRace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HorseRace.Controllers
{
    public class WyscigiController : Controller
    {
        private readonly HorseRaceContext _context;

        public WyscigiController(HorseRaceContext context)
        {
            _context = context;
        }
        // GET: /Wyscigi
        public async Task<IActionResult> Index()
        {
            var wyscigi = await _context.Wyscigi.Include(w => w.Wlasciciel).ToListAsync();
            return View(wyscigi);
        }

        [HttpGet]
        public IActionResult DodajWyscig()
        {
            ViewBag.Konie = _context.Konie
                .Select(k => new SelectListItem
                {
                    Value = k.Id.ToString(),
                    Text = k.Nazwa
                })
                .ToList();
            ViewBag.Poziomy = new SelectList(Enum.GetValues(typeof(PoziomTrudnosci)));
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajWyscig(Wyscig wyscig, int[] wybraneKonie)
        {
            Console.WriteLine(wybraneKonie.Length);
            if (ModelState.IsValid && wybraneKonie.Length == 5)
            {
                var konie = _context.Konie.Where(k => wybraneKonie.Contains(k.Id)).ToList();
                wyscig.Konie = konie;
                wyscig.WlascicielId = 1; // Tymczasowo przypisany ID admina lub zalogowanego

                _context.Wyscigi.Add(wyscig);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Poziomy = new SelectList(Enum.GetValues(typeof(PoziomTrudnosci)));
            ViewBag.Konie = _context.Konie
                .Select(k => new SelectListItem
                {
                    Value = k.Id.ToString(),
                    Text = k.Nazwa
                })
                .ToList();
            ViewBag.KonieError = "Musisz wybrać dokładnie 5 koni.";
            return View(wyscig);
        }
    }
}

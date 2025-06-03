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

        [HttpGet]
        public IActionResult EdytujWyscig(int id)
        {
            var wyscig = _context.Wyscigi
                .Include(w => w.Konie)
                .FirstOrDefault(w => w.Id == id);

            if (wyscig == null)
            {
                return NotFound();
            }

            var wszystkieKonie = _context.Konie.ToList();
            var przypisaneKonieIds = wyscig.Konie.Select(k => k.Id).ToHashSet();

            ViewBag.Konie = wszystkieKonie
                .Select(k => new SelectListItem
                {
                    Value = k.Id.ToString(),
                    Text = k.Nazwa,
                    Selected = przypisaneKonieIds.Contains(k.Id)
                })
                .ToList();

            return View(wyscig);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujWyscig(int id, Wyscig wyscig, int[] wybraneKonie)
        {
            var wyscigDoAktualizacji = _context.Wyscigi
                .Include(w => w.Konie)
                .FirstOrDefault(w => w.Id == id);

            if (wyscigDoAktualizacji == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid && wybraneKonie.Length == 5)
            {
                wyscigDoAktualizacji.Nazwa = wyscig.Nazwa;
                wyscigDoAktualizacji.Koszt = wyscig.Koszt;
                wyscigDoAktualizacji.Nagroda = wyscig.Nagroda;
                wyscigDoAktualizacji.PoziomTrudnosci = wyscig.PoziomTrudnosci;

                // Zmiana przypisanych koni
                wyscigDoAktualizacji.Konie.Clear();
                var noweKonie = _context.Konie.Where(k => wybraneKonie.Contains(k.Id)).ToList();
                foreach (var kon in noweKonie)
                {
                    wyscigDoAktualizacji.Konie.Add(kon);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Konie = _context.Konie
                .Select(k => new SelectListItem
                {
                    Value = k.Id.ToString(),
                    Text = k.Nazwa,
                    Selected = wybraneKonie.Contains(k.Id)
                })
                .ToList();

            ViewBag.KonieError = wybraneKonie.Length != 5 ? "Wybierz dokładnie 5 koni." : null;
            return View(wyscig);
        }
        [HttpGet]
        public async Task<IActionResult> UsunWyscig(int? id)
        {
            if (id == null)
                return NotFound();

            var wyscig = await _context.Wyscigi
                .Include(w => w.Wlasciciel)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wyscig == null)
                return NotFound();

            return View(wyscig);
        }

        [HttpPost, ActionName("UsunWyscig")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var wyscig = await _context.Wyscigi
                .Include(w => w.Konie) 
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wyscig != null)
            {
                wyscig.Konie.Clear(); // odłącz konie
                _context.Wyscigi.Remove(wyscig);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> SzczegolyWyscig(int? id)
        {
            if (id == null)
                return NotFound();

            var wyscig = await _context.Wyscigi
                .Include(w => w.Wlasciciel)
                .Include(w => w.Konie)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wyscig == null)
                return NotFound();

            return View(wyscig);
        }

        [HttpGet]
        public async Task<IActionResult> Statystyka()
        {
            var liczbaWyscigow = await _context.Wyscigi.CountAsync();
            var sredniaNagroda = await _context.Wyscigi.AverageAsync(w => w.Nagroda);
            var liczbaZrealizowanych = await _context.Wyscigi.CountAsync(w => w.CzyZrealizowany);
            var sredniaPredkosc = await _context.Wyscigi.Include(w => w.Konie).SelectMany(w => w.Konie).AverageAsync(w => w.MaxSzybkosc);
            var sredniaWytrzymalosc = await _context.Wyscigi.Include(w => w.Konie).SelectMany(w => w.Konie).AverageAsync(w => w.MaxWytrzymalosc);

            ViewBag.LiczbaWyscigow = liczbaWyscigow;
            ViewBag.SredniaNagroda = sredniaNagroda;
            ViewBag.Zrealizowane = liczbaZrealizowanych;
            ViewBag.SredniaPredkosc = Math.Round(sredniaPredkosc);
            ViewBag.SredniaWytrzymalosc = Math.Round(sredniaWytrzymalosc);
            return View();
        }

    }
}

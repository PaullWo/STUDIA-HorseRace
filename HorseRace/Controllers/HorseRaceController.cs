using HorseRace.Data;
using HorseRace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HorseRace.Controllers
{
    public class HorseRaceController : Controller
    {
        private readonly HorseRaceContext _context;

        public HorseRaceController(HorseRaceContext context)
        {
            _context = context;
        }

        //GET /
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //POST /
        [HttpPost]
        public IActionResult Index(string login, string haslo)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(u => u.Login == login
            && u.Haslo == haslo);
            if ( uzytkownik != null)
            {
                if (uzytkownik.CzyAdmin)
                {
                    return RedirectToAction("PanelAdmin", "HorseRace");
                }
                else
                {
                    if (uzytkownik.CzyMaKonia == false)
                    {
                        return RedirectToAction("NowyUserPanel", "HorseRace", new { id = uzytkownik.Id });
                    }
                    else {
                        return RedirectToAction("PanelUser", "HorseRace", new { id = uzytkownik.Id });
                    }
                }
            }
            ViewBag.Error = "Nieprawidłowy login lub hasło.";
            return View();
        }
        [HttpGet]
        public IActionResult NowyUserPanel(int id)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(u => u.Id == id);
            return View(uzytkownik);
        }
        [HttpPost]
        public IActionResult NowyUserPanel(int id,string WybranyKolor,string nazwa)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(u => u.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            else
            {
                Umaszczenie umaszczenie = Enum.Parse<Umaszczenie>(WybranyKolor);
                Kon kon = new Kon { Nazwa = nazwa, Umaszczenie = umaszczenie, MaxWytrzymalosc = 100, MaxSzybkosc = 100, Wlasciciel=uzytkownik };
                _context.Konie.Add(kon);
                uzytkownik.CzyMaKonia = true;
                _context.SaveChanges();
            }
                return RedirectToAction("PanelUser", "HorseRace", new { id = uzytkownik.Id });
        }
        [HttpGet]
        public IActionResult Rejestracja()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Rejestracja(string login, string haslo, string potwierdzHaslo)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(haslo))
            {
                ViewBag.Error = "Wszystkie pola są wymagane.";
                return View();
            }

            if (haslo != potwierdzHaslo)
            {
                ViewBag.Error = "Hasła nie są takie same.";
                return View();
            }

            if (_context.Uzytkownicy.Any(u => u.Login == login))
            {
                ViewBag.Error = "Ten login jest już zajęty.";
                return View();
            }

            var nowyUzytkownik = new Uzytkownik
            {
                Login = login,
                Haslo = haslo,
                CzyAdmin = false,
                DataDolaczenia = DateTime.Now
            };

            _context.Uzytkownicy.Add(nowyUzytkownik);
            _context.SaveChanges();

            TempData["RejestracjaSukces"] = "Rejestracja zakończona pomyślnie!.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult PanelAdmin(int id)
        {
            return View();
        }
        [HttpGet]
        public IActionResult PanelUser()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Tor()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Stajnia(string searchString, string sortOrder, int page = 1, int pageSize = 5)
        {
            var konie = _context.Konie.AsQueryable();

            // WYSZUKIWANIE
            if (!string.IsNullOrEmpty(searchString))
            {
                konie = konie.Where(k => k.Nazwa.Contains(searchString));
            }

            // SORTOWANIE
            ViewBag.NameSort = sortOrder == "name_desc" ? "name_asc" : "name_desc";
            ViewBag.SpeedSort = sortOrder == "speed_desc" ? "speed_asc" : "speed_desc";

            switch (sortOrder)
            {
                case "name_desc":
                    konie = konie.OrderByDescending(k => k.Nazwa);
                    break;
                case "name_asc":
                    konie = konie.OrderBy(k => k.Nazwa);
                    break;
                case "speed_desc":
                    konie = konie.OrderByDescending(k => k.MaxSzybkosc);
                    break;
                case "speed_asc":
                    konie = konie.OrderBy(k => k.MaxSzybkosc);
                    break;
                default:
                    konie = konie.OrderBy(k => k.Id);
                    break;
            }

            // STRONICOWANIE
            var totalItems = konie.Count();
            var konieNaStronie = konie.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;

            return View(konieNaStronie);
        }


        [HttpGet]
        public IActionResult DodajKonia()
        {
            ViewBag.UmaszczenieList = new SelectList(Enum.GetValues(typeof(Umaszczenie)));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajKonia([Bind("Nazwa,Umaszczenie,MaxWytrzymalosc,MaxSzybkosc")] Kon kon)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.Uzytkownicy.FirstOrDefault(u => u.Login == "admin");
                kon.Wlasciciel = admin;
                _context.Add(kon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Stajnia));
            }

           
            Console.WriteLine("BŁAD");
            ViewBag.UmaszczenieList = new SelectList(Enum.GetValues(typeof(Umaszczenie)));
            return View(kon);
        }

        [HttpGet]
        public IActionResult EdytujKonia(int id)
        {
            var kon = _context.Konie.FirstOrDefault(k => k.Id == id);
            if (kon == null)
            {
                return NotFound();
            }

            ViewBag.UmaszczenieList = new SelectList(Enum.GetValues(typeof(Umaszczenie)));
            return View(kon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujKonia(int id, [Bind("Id,Nazwa,Umaszczenie,MaxWytrzymalosc,MaxSzybkosc")] Kon kon)
        {
            if (id != kon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var konZBazy = await _context.Konie.FindAsync(id);
                if (konZBazy == null)
                {
                    return NotFound();
                }
                konZBazy.Nazwa = kon.Nazwa;
                konZBazy.Umaszczenie = kon.Umaszczenie;
                konZBazy.MaxWytrzymalosc = kon.MaxWytrzymalosc;
                konZBazy.MaxSzybkosc = kon.MaxSzybkosc;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Stajnia));
            }

            ViewBag.UmaszczenieList = new SelectList(Enum.GetValues(typeof(Umaszczenie)));
            return View(kon);
        }

        [HttpGet]
        public IActionResult UsunKonia(int id)
        {
            var kon = _context.Konie.FirstOrDefault(k => k.Id == id);
            if (kon == null)
            {
                return NotFound();
            }

            return View(kon);
        }

        [HttpPost, ActionName("UsunKonia")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunKoniaPotwierdzone(int id)
        {
            var kon = await _context.Konie.FindAsync(id);
            if (kon == null)
            {
                return NotFound();
            }

            _context.Konie.Remove(kon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Stajnia));
        }

        [HttpGet]
        public async Task<IActionResult> SzczegolyKonia(int id)
        {
            var kon = _context.Konie.FirstOrDefault(k => k.Id == id);
            if (kon == null)
            {
                return NotFound();
            }
            var wyscigi = await _context.Konie
            .Include(w => w.Wyscigi)
            .FirstOrDefaultAsync(w => w.Id == id);
            ViewBag.WyscigiList = wyscigi;
            return View(kon);
        }





    }
}

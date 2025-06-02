using HorseRace.Data;
using HorseRace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    return RedirectToAction("PanelUser", "HorseRace");
                }
            }
            ViewBag.Error = "Nieprawidłowy login lub hasło.";
            return View();
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

            // przekierowanie np. do logowania
            TempData["RejestracjaSukces"] = "Rejestracja zakończona pomyślnie!.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult PanelAdmin()
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
        public IActionResult Stajnia()
        {
            var konie = _context.Konie.ToList();
            return View(konie);
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
                _context.Add(kon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Stajnia));
            }

           
            Console.WriteLine("BŁAD");
            return View(kon);
        }



    }
}

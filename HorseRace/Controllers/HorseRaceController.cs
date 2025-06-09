using BibliotekaWspolna;
using HorseRace.Data;
using HorseRace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
            if (uzytkownik != null)
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
                    else
                    {
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
        public IActionResult NowyUserPanel(int id, string WybranyKolor, string nazwa)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(u => u.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            else
            {
                Umaszczenie umaszczenie = Enum.Parse<Umaszczenie>(WybranyKolor);
                Kon kon = new Kon { Nazwa = nazwa, Umaszczenie = umaszczenie, MaxWytrzymalosc = 100, MaxSzybkosc = 100, Wlasciciel = uzytkownik };
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
        public IActionResult PanelUser(int id, string sortOrder = "", int page = 1, int pageSize = 3)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(u => u.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            ViewBag.Kon = _context.Konie.FirstOrDefault(k => k.WlascicielId == id);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "nazwa_desc" : "";
            ViewBag.LevelSortParam = sortOrder == "poziom" ? "poziom_desc" : "poziom";

            var wyscigiQuery = _context.Wyscigi
                .Where(w => !w.CzyZrealizowany);

            // Sortowanie
            switch (sortOrder)
            {
                case "nazwa_desc":
                    wyscigiQuery = wyscigiQuery.OrderByDescending(w => w.Nazwa);
                    break;
                case "poziom":
                    wyscigiQuery = wyscigiQuery.OrderBy(w => w.PoziomTrudnosci);
                    break;
                case "poziom_desc":
                    wyscigiQuery = wyscigiQuery.OrderByDescending(w => w.PoziomTrudnosci);
                    break;
                default:
                    wyscigiQuery = wyscigiQuery.OrderBy(w => w.Nazwa);
                    break;
            }

            // Paginacja
            int totalItems = wyscigiQuery.Count();
            var wyscigi = wyscigiQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new
                {
                    w.Id,
                    w.Nazwa,
                    w.Koszt,
                    w.Nagroda,
                    w.PoziomTrudnosci
                })
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Wyscigi = wyscigi;

            return View(uzytkownik);
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
            var kon = await _context.Konie
            .Include(k => k.Wyscigi)
            .FirstOrDefaultAsync(k => k.Id == id);
            if (kon == null)
            {
                return NotFound();
            }
            if (kon.Wyscigi != null && kon.Wyscigi.Count > 0)
            {
                TempData["KomunikatAlert"] = "Nie mozna usunac konia, poniewaz bierze udzial w wyscigu!";
                return RedirectToAction(nameof(Stajnia));
            }
            var wlasciciel = await _context.Uzytkownicy.FirstOrDefaultAsync(u => u.Id == kon.WlascicielId);
            if (wlasciciel != null && !wlasciciel.CzyAdmin)
            {
                wlasciciel.CzyMaKonia = false;
                _context.Uzytkownicy.Update(wlasciciel);
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


        [HttpGet]
        public async Task<IActionResult> PracaZlom(int id)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(k => k.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            var teraz = DateTime.Now;

            if (uzytkownik.OstatniaPracaZlom != null && (teraz - uzytkownik.OstatniaPracaZlom.Value).TotalMinutes < 60)
            {
                var pozostało = 60 - (teraz - uzytkownik.OstatniaPracaZlom.Value).TotalMinutes;
                TempData["KomunikatPraca"] = $"Cały złom wyprzedany…  <br> Spróbuj ponownie za <strong>{Math.Ceiling(pozostało)} minut.</strong>";
                return RedirectToAction("PanelUser", "HorseRace", new { id = uzytkownik.Id });
            }

            var rng = new Random();
            int zarobek = rng.Next(50, 101);

            string[] komunikaty =
            {
            "W stercie złomu znalazłeś coś błyszczącego – i opłaciło się!",
            "Ktoś rozbił końmobil – zebrałeś mnóstwo części!",
            "Znalazłeś leżący silnik V8 – koń był zachwycony!"
            };
            string komunikat = komunikaty[rng.Next(komunikaty.Length)];
            uzytkownik.ZlotePodkowy += zarobek;
            uzytkownik.OstatniaPracaZlom = teraz;
            _context.SaveChanges();
            TempData["KomunikatPraca"] = $"{komunikat} <br><strong>+{zarobek} <img src='/images/podkowa.png' class='zlota_podkowa' style='height: 20px;margin-top:20px;' /> </strong>";

            return RedirectToAction("PanelUser", "HorseRace", new { id = uzytkownik.Id });
        }

        [HttpGet]
        public async Task<IActionResult> PracaStajnia(int id)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(k => k.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            var teraz = DateTime.Now;

            if (uzytkownik.OstatniaPracaStajnia != null && (teraz - uzytkownik.OstatniaPracaStajnia.Value).TotalMinutes < 60)
            {
                var pozostało = 60 - (teraz - uzytkownik.OstatniaPracaStajnia.Value).TotalMinutes;
                TempData["KomunikatPraca"] = $"Konie muszą odpocząć…  <br> Spróbuj ponownie za <strong>{Math.Ceiling(pozostało)} minut.</strong>";
                return RedirectToAction("PanelUser", "HorseRace", new { id = uzytkownik.Id });
            }

            var rng = new Random();
            int zarobek = rng.Next(100, 201);

            string[] komunikaty =
            {
            "Uzupełniłeś zapasy paszy i oleju silnikowego. <br> Konie zadowolone, stajnia błyszczy.",
            "Wyczyściłeś podkowy wszystkim koniom w stajni. <br>Grand Master dorzucił napiwek.",
            "Znalazłeś zagubioną śrubę od starego Mustanga. <br> Podobno bez niej nie odpala."
            };
            string komunikat = komunikaty[rng.Next(komunikaty.Length)];
            uzytkownik.ZlotePodkowy += zarobek;
            uzytkownik.OstatniaPracaStajnia = teraz;
            _context.SaveChanges();
            TempData["KomunikatPraca"] = $"{komunikat} <br><strong>+{zarobek} <img src='/images/podkowa.png' class='zlota_podkowa' style='height: 20px;margin-top:20px;' /> </strong>";

            return RedirectToAction("PanelUser", "HorseRace", new { id = uzytkownik.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Ranking(int id)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(k => k.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            var wszystkieKonie = await _context.Konie
                .OrderByDescending(k => k.LiczbaWygranychWyscigow)
                .ToListAsync();

            ViewBag.TopKonie = wszystkieKonie.Take(3).ToList();

            var konGracza = wszystkieKonie.FirstOrDefault(k => k.WlascicielId == id);

            int? miejsce = null;
            if (konGracza != null)
            {
                miejsce = wszystkieKonie.IndexOf(konGracza) + 1;
            }

            ViewBag.MiejsceGracza = miejsce;

            return View(uzytkownik);
        }

        [HttpGet]
        public async Task<IActionResult> Wyscig(int id, int wyscigID)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(k => k.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            var wyscig = _context.Wyscigi
            .Include(w => w.Konie)
            .FirstOrDefault(w => w.Id == wyscigID);
            if (wyscig == null)
            {
                return NotFound();
            }
            var kon = _context.Konie.FirstOrDefault(k => k.WlascicielId == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            uzytkownik.ZlotePodkowy -= wyscig.Koszt;
            wyscig.Konie.Add(kon);
            _context.SaveChanges();

            var konie = _context.Konie.Where(k => wyscig.Konie.Contains(k)).ToList();
            ViewBag.Konie = konie;

            ViewBag.Wyscig = wyscig;

            return View(uzytkownik);
        }

        [HttpGet]
        public async Task<IActionResult> Wyniki(int id, int wyscigId, int zwyciezcaId)
        {
            var uzytkownik = _context.Uzytkownicy.FirstOrDefault(k => k.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            var wyscig = _context.Wyscigi
            .Include(w => w.Konie)
            .FirstOrDefault(w => w.Id == wyscigId);
            if (wyscig == null)
            {
                return NotFound();
            }
            wyscig.CzyZrealizowany = true;
            ViewBag.Wyscig = wyscig;
            var kon = _context.Konie.FirstOrDefault(k => k.WlascicielId == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            if (zwyciezcaId == 5)
            {
                ViewBag.Komunikat1 = "Gratulacje! Twój koń wygrał wyścig!";
                ViewBag.Komunikat2 = "Wygrywasz złote podkowy, oraz łapiesz doświadczenie.<br>" +
                    "<strong>+" + wyscig.Nagroda + " <img src='/images/podkowa.png' style='height: 18px;' /></strong><br>" +
                    "<strong>+40 km/h</strong><br><strong>+20 ml/kg/min</strong><br>";
                kon.LiczbaWygranychWyscigow += 1;
                kon.MaxSzybkosc += 40;
                kon.MaxWytrzymalosc += 20;
                uzytkownik.ZlotePodkowy += wyscig.Nagroda;
            }
            else
            {
                ViewBag.Komunikat1 = "Porażka! Twój koń przegrał wyścig...";
                ViewBag.Komunikat2 = "Tracisz złote podkowy, ale łapiesz doświadczenie.<br>" +
                    "<strong>-" + wyscig.Koszt + " <img src='/images/podkowa.png' style='height: 18px;' /></strong><br>" +
                    "<strong>+20 km/h</strong><br><strong>+10 ml/kg/min</strong><br>";
                kon.MaxSzybkosc += 20;
                kon.MaxWytrzymalosc += 10;
            }
            _context.SaveChanges();
            return View(uzytkownik);
        }

        [HttpGet]
        public async Task<IActionResult> Wtyczki()
        {
            var adminId = _context.Uzytkownicy
            .Where(u => u.Login == "admin")
            .Select(u => u.Id)
            .FirstOrDefault();
            string nazwaPakietu = "";
            string pluginsPath = Path.Combine(AppContext.BaseDirectory, "Plugins");
            var konieZDll = new List<KonPodstawowy>();

            if (Directory.Exists(pluginsPath))
            {
                var dllPaths = Directory.GetFiles(pluginsPath, "*.dll");

                foreach (var dll in dllPaths)
                {
                    try
                    {
                        var asm = Assembly.LoadFrom(dll);
                        var typy = asm.GetTypes()
                            .Where(t => typeof(IListaKoni).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                        foreach (var typ in typy)
                        {
                            if (Activator.CreateInstance(typ) is IListaKoni instancja)
                            {
                                konieZDll.AddRange(instancja.zwrocKonie());
                                var attr = typ.GetCustomAttribute<InfoAttribute>();
                                nazwaPakietu = attr?.NazwaPakietu ?? "Nieznany pakiet";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd ładowania DLL {dll}: {ex.Message}");
                    }
                }

                foreach (var konDll in konieZDll)
                {
                    bool czyIstnieje = _context.Konie.Any(k =>
                        k.Nazwa == konDll.Nazwa &&
                        k.MaxSzybkosc == konDll.MaxSzybkosc &&
                        k.MaxWytrzymalosc == konDll.MaxWytrzymalosc &&
                        k.Umaszczenie == konDll.Umaszczenie &&
                        k.WlascicielId == adminId
                    );

                    if (!czyIstnieje)
                    {
                        var nowyKon = new Kon
                        {
                            Nazwa = konDll.Nazwa,
                            MaxSzybkosc = konDll.MaxSzybkosc,
                            MaxWytrzymalosc = konDll.MaxWytrzymalosc,
                            Umaszczenie = konDll.Umaszczenie,
                            WlascicielId = adminId
                        };

                        _context.Konie.Add(nowyKon);
                        Console.WriteLine($"===[PAKIET]: {nazwaPakietu} – załadowano konia.===");
                    }
                    else
                    {
                        Console.WriteLine($"===[PAKIET]: {nazwaPakietu} – pominięto duplikat konia: {konDll.Nazwa}===");
                    }
                }

                _context.SaveChanges();

                Console.WriteLine($"===Dodano {konieZDll.Count} koni z pakietów DLL.===");
                return RedirectToAction("PanelAdmin");
            }

            return RedirectToAction("PanelAdmin");
        }
    }
}

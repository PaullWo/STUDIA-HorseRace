using HorseRace.Data;
using Microsoft.AspNetCore.Mvc;

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
            if (login == "admin" && haslo == "admin")
            {
                return RedirectToAction("PanelAdmin", "HorseRace");
            }

            ViewBag.Error = "Nieprawidłowy login lub hasło.";
            return View();
        }

        [HttpGet]
        public IActionResult Rejestracja()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PanelAdmin()
        {
            return View();
        }
    }
}

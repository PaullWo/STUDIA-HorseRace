using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HorseRace.Models
{
    public class Uzytkownik
    {
		public Uzytkownik()
		{
            CzyMaKonia = false; // Domyślna wartość
            Wyscigi = new List<Wyscig>();
			ZlotePodkowy = 1000;
        }
		//Id
		private int _id;
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		//Login
		private string _login;
		public string Login
		{
			get { return _login; }
			set { _login = value; }
		}

		//Haslo
		private string _haslo;
		public string Haslo
		{
			get { return _haslo; }
			set { _haslo = value; }
		}

		//Czy jest adminem
		private bool _czyAdmin;
		public bool CzyAdmin
		{
			get { return _czyAdmin; }
			set { _czyAdmin = value; }
		}

		//Data dołączenia
		private DateTime _dataDolaczenia;
		public DateTime DataDolaczenia
		{
			get { return _dataDolaczenia; }
			set { _dataDolaczenia = value; }
		}

		//Kolekcja wyścigów
		[ValidateNever]
		public ICollection<Wyscig> Wyscigi { get; set; }

		private bool _czyMaKonia;

		public bool CzyMaKonia
		{
			get { return _czyMaKonia; }
			set { _czyMaKonia = value; }
		}

		//Zlote Podkowy waluta
		private int _zlotePodkowy;

		public int ZlotePodkowy
		{
			get { return _zlotePodkowy; }
			set { _zlotePodkowy = value; }
		}

        //Czas ostatniego pracowania - złom
        [ValidateNever]
        public DateTime? OstatniaPracaZlom { get; set; }

        //Czas ostatniego pracowania - stajnia
        [ValidateNever]
        public DateTime? OstatniaPracaStajnia { get; set; }
    }
}

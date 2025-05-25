namespace HorseRace.Models
{
    public class Uzytkownik
    {
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
		public ICollection<Wyscig> Wyscigi { get; set; }
	}
}

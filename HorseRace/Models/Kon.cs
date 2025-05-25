namespace HorseRace.Models
{
    public class Kon
    {
		//Id
		private int _id;
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		//Nazwa
		private string _nazwa;
		public string Nazwa
		{
			get { return _nazwa; }
			set { _nazwa = value; }
		}

		//Umaszczenie -> enum
		private Umaszczenie _umaszczenie;
		public Umaszczenie Umaszczenie
		{
			get { return _umaszczenie; }
			set { _umaszczenie = value; }
		}

		//Wytrzymałość
		private int _wytrzymalosc;
		public int Wytrzymalosc
		{
			get { return _wytrzymalosc; }
			set { _wytrzymalosc = value; }
		}

		//Maksymalna wytrzymałość
		private int _maxWytrzymalosc;
		public int MaxWytrzymalosc
		{
			get { return _maxWytrzymalosc; }
			set { _maxWytrzymalosc = value; }
		}

		//Szybkość
		private int _szybkosc;
		public int Szybkosc
		{
			get { return _szybkosc; }
			set { _szybkosc = value; }
		}

		//Maksymalna szybkość
		private int _maxSzybkosc;
		public int MaxSzybkosc
		{
			get { return _maxSzybkosc; }
			set { _maxSzybkosc = value; }
		}

        //Kolekcja wyscigow
        public ICollection<Wyscig> Wyscigi { get; set; }




    }
}

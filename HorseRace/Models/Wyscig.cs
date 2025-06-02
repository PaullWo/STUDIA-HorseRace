using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace HorseRace.Models
{
    public class Wyscig
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

		//Wlasciciel
		private int _wlascicielId;
        [ValidateNever]
        public int WlascicielId
		{
			get { return _wlascicielId; }
			set { _wlascicielId = value; }
		}

		private Uzytkownik _wlasciciel;
        [ValidateNever]
        public Uzytkownik Wlasciciel
		{
			get { return _wlasciciel; }
			set { _wlasciciel = value; }
		}

		//Koszt za dołączenie
		private int _koszt;
		public int Koszt
		{
			get { return _koszt; }
			set { _koszt = value; }
		}

		//Nagroda za zwycięstwo
		private int _nagroda;
		public int Nagroda
		{
			get { return _nagroda; }
			set { _nagroda = value; }
		}

        //Kolekcja koni (botów) biorących udział w wyścigu
        [ValidateNever]
        public ICollection<Kon> Konie { get; set; }


        //Czy wyścig już się odbył
        private bool _czyZrealizowany;
        [ValidateNever]
        public bool CzyZrealizowany
		{
			get { return _czyZrealizowany; }
			set { _czyZrealizowany = value; }
		}

		//Poziom trudności
		private PoziomTrudnosci _poziomTrudnosci;

		public PoziomTrudnosci PoziomTrudnosci
		{
			get { return _poziomTrudnosci; }
			set { _poziomTrudnosci = value; }
		}


	}
}

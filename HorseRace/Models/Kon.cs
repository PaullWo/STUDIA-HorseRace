﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BibliotekaWspolna;

namespace HorseRace.Models
{
    public class Kon : KonPodstawowy
    {
        public Kon() {
            LiczbaWygranychWyscigow = 0;
        }
        /*
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Nazwa' jest wymagane.")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Pole 'Umaszczenie' jest wymagane.")]
        public Umaszczenie Umaszczenie { get; set; }

        [Required(ErrorMessage = "Podaj maksymalną wytrzymałość.")]
        [Range(1, int.MaxValue, ErrorMessage = "Wytrzymałość musi być większa od zera.")]
        public int MaxWytrzymalosc { get; set; }

        [Required(ErrorMessage = "Podaj maksymalną szybkość.")]
        [Range(1, int.MaxValue, ErrorMessage = "Szybkość musi być większa od zera.")]
        public int MaxSzybkosc { get; set; }
        */
        [ValidateNever]
        public ICollection<Wyscig> Wyscigi { get; set; }

        //Wlasciciel
        public int WlascicielId { get; set; }

        [ValidateNever]
        public int LiczbaWygranychWyscigow { get; set; }

        [ValidateNever]
        public Uzytkownik Wlasciciel { get; set; }

    }
}

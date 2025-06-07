using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaWspolna
{
    public class KonPodstawowy
    {
        public KonPodstawowy()
        {
            
        }
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

    }
}

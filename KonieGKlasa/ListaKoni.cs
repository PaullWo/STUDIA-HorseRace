using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotekaWspolna;
namespace KonieGKlasa
{
    [Info(NazwaPakietu = "GKlasa")]
    public class ListaKoni : IListaKoni
    {
        private List<KonPodstawowy> lista;
        public ListaKoni()
        {
            lista= new List<KonPodstawowy>
            {
            new KonPodstawowy{Nazwa="AMG Spirit",Umaszczenie=Umaszczenie.Bialy,MaxWytrzymalosc=130,MaxSzybkosc=120},
            new KonPodstawowy{Nazwa="Gelandewagen",Umaszczenie=Umaszczenie.Czarny,MaxWytrzymalosc=200,MaxSzybkosc=110},
            new KonPodstawowy{Nazwa="G63",Umaszczenie=Umaszczenie.Czarny,MaxWytrzymalosc=150,MaxSzybkosc=100},
            new KonPodstawowy{Nazwa="G-Force",Umaszczenie=Umaszczenie.Czarny,MaxWytrzymalosc=200,MaxSzybkosc=150},
            new KonPodstawowy{Nazwa="G-Racer",Umaszczenie=Umaszczenie.Bialy,MaxWytrzymalosc=150,MaxSzybkosc=120}
            };
        }
        public int WielkoscPakietu()
        {
            return lista.Count;
        }

        public List<KonPodstawowy> zwrocKonie()
        {
            return lista;
        }
    }
}

using System.Diagnostics;
using HorseRace.Models;
using System;
using System.Linq;

namespace HorseRace.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HorseRaceContext context)
        {
            context.Database.EnsureCreated();

            if (context.Konie.Any())
            {
                return;  
            }

            var konie = new Kon[]
            {
            new Kon{Nazwa="ToyotaYaris",Umaszczenie=Umaszczenie.Toyota,MaxWytrzymalosc=200,MaxSzybkosc=70},
            new Kon{Nazwa="Kasztanka",Umaszczenie=Umaszczenie.Brazowy,MaxWytrzymalosc=250,MaxSzybkosc=50},
            new Kon{Nazwa="Mustang",Umaszczenie=Umaszczenie.Czarny,MaxWytrzymalosc=150,MaxSzybkosc=90}
            };
            foreach (Kon k in konie)
            {
                context.Konie.Add(k);
            }
            context.SaveChanges();

            var uzytkownicy = new Uzytkownik[]
            {
            new Uzytkownik{Login="admin",Haslo="admin",CzyAdmin=true,DataDolaczenia=DateTime.Parse("2025-05-25")}
            };
            foreach (Uzytkownik u in uzytkownicy)
            {
                context.Uzytkownicy.Add(u);
            }
            context.SaveChanges();

            var wyscigi = new Wyscig[]
            {
            new Wyscig{Nazwa="Grand Prix Monako",WlascicielId=1,Koszt=100,Nagroda=1000,CzyZrealizowany=false,PoziomTrudnosci=PoziomTrudnosci.GrandPrix}
            };
            foreach (Wyscig w in wyscigi)
            {
                context.Wyscigi.Add(w);
            }
            context.SaveChanges();
        }
    }
}

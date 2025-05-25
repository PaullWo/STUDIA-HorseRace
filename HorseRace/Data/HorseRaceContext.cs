using HorseRace.Models;
using Microsoft.EntityFrameworkCore;

namespace HorseRace.Data
{
    public class HorseRaceContext : DbContext
    {
        public HorseRaceContext(DbContextOptions<HorseRaceContext> options) : base(options)
        {

        }

        public DbSet<Kon> Konie { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Wyscig> Wyscigi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kon>().ToTable("Kon");
            modelBuilder.Entity<Uzytkownik>().ToTable("Uzytkownik");
            modelBuilder.Entity<Wyscig>().ToTable("Wyscig");
        }
    }
}

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

            // Konfiguracja relacji wiele-do-wielu bez Cascade Delete
            modelBuilder.Entity<Kon>()
                .HasMany(k => k.Wyscigi)
                .WithMany(w => w.Konie)
                .UsingEntity<Dictionary<string, object>>(
                    "KonWyscig", // nazwa tabeli pośredniej
                    j => j.HasOne<Wyscig>().WithMany().HasForeignKey("WyscigiId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Kon>().WithMany().HasForeignKey("KonieId").OnDelete(DeleteBehavior.Restrict)
                );
        }

    }
}

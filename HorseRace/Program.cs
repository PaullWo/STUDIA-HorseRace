using HorseRace.Data;
using Microsoft.EntityFrameworkCore;

namespace HorseRace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //Dodawanie bazy danych
            builder.Services.AddDbContext<HorseRaceContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("HorseRaceContext")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Tworzenie bazy danych jeœli nie istnieje
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<HorseRaceContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Wyst¹pi³ b³¹d przy tworzeniu bazy danych.");
                }
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=HorseRace}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

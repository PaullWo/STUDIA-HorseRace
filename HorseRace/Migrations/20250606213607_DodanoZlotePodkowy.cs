using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorseRace.Migrations
{
    /// <inheritdoc />
    public partial class DodanoZlotePodkowy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZlotePodkowy",
                table: "Uzytkownik",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZlotePodkowy",
                table: "Uzytkownik");
        }
    }
}

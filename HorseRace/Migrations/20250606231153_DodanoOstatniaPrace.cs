using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorseRace.Migrations
{
    /// <inheritdoc />
    public partial class DodanoOstatniaPrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OstatniaPracaStajnia",
                table: "Uzytkownik",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OstatniaPracaZlom",
                table: "Uzytkownik",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OstatniaPracaStajnia",
                table: "Uzytkownik");

            migrationBuilder.DropColumn(
                name: "OstatniaPracaZlom",
                table: "Uzytkownik");
        }
    }
}

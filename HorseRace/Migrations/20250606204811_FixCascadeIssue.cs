using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorseRace.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uzytkownik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Haslo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CzyAdmin = table.Column<bool>(type: "bit", nullable: false),
                    DataDolaczenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CzyMaKonia = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Umaszczenie = table.Column<int>(type: "int", nullable: false),
                    MaxWytrzymalosc = table.Column<int>(type: "int", nullable: false),
                    MaxSzybkosc = table.Column<int>(type: "int", nullable: false),
                    WlascicielId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kon_Uzytkownik_WlascicielId",
                        column: x => x.WlascicielId,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wyscig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WlascicielId = table.Column<int>(type: "int", nullable: false),
                    Koszt = table.Column<int>(type: "int", nullable: false),
                    Nagroda = table.Column<int>(type: "int", nullable: false),
                    CzyZrealizowany = table.Column<bool>(type: "bit", nullable: false),
                    PoziomTrudnosci = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wyscig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wyscig_Uzytkownik_WlascicielId",
                        column: x => x.WlascicielId,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KonWyscig",
                columns: table => new
                {
                    KonieId = table.Column<int>(type: "int", nullable: false),
                    WyscigiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KonWyscig", x => new { x.KonieId, x.WyscigiId });
                    table.ForeignKey(
                        name: "FK_KonWyscig_Kon_KonieId",
                        column: x => x.KonieId,
                        principalTable: "Kon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KonWyscig_Wyscig_WyscigiId",
                        column: x => x.WyscigiId,
                        principalTable: "Wyscig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kon_WlascicielId",
                table: "Kon",
                column: "WlascicielId");

            migrationBuilder.CreateIndex(
                name: "IX_KonWyscig_WyscigiId",
                table: "KonWyscig",
                column: "WyscigiId");

            migrationBuilder.CreateIndex(
                name: "IX_Wyscig_WlascicielId",
                table: "Wyscig",
                column: "WlascicielId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KonWyscig");

            migrationBuilder.DropTable(
                name: "Kon");

            migrationBuilder.DropTable(
                name: "Wyscig");

            migrationBuilder.DropTable(
                name: "Uzytkownik");
        }
    }
}

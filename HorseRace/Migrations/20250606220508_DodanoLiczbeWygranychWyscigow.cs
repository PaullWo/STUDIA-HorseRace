using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorseRace.Migrations
{
    /// <inheritdoc />
    public partial class DodanoLiczbeWygranychWyscigow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LiczbaWygranychWyscigow",
                table: "Kon",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiczbaWygranychWyscigow",
                table: "Kon");
        }
    }
}

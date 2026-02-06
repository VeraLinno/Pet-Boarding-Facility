using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBreedAndUseEnumSpecies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Breed",
                table: "Pets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Breed",
                table: "Pets",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}

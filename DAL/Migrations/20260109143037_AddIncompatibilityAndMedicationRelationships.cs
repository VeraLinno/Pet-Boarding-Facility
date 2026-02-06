using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIncompatibilityAndMedicationRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kennels_Pets_CurrentPetId1",
                table: "Kennels");

            migrationBuilder.DropIndex(
                name: "IX_Kennels_CurrentPetId1",
                table: "Kennels");

            migrationBuilder.DropColumn(
                name: "CurrentPetId1",
                table: "Kennels");

            migrationBuilder.CreateIndex(
                name: "IX_Kennels_CurrentPetId",
                table: "Kennels",
                column: "CurrentPetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kennels_Pets_CurrentPetId",
                table: "Kennels",
                column: "CurrentPetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kennels_Pets_CurrentPetId",
                table: "Kennels");

            migrationBuilder.DropIndex(
                name: "IX_Kennels_CurrentPetId",
                table: "Kennels");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentPetId1",
                table: "Kennels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kennels_CurrentPetId1",
                table: "Kennels",
                column: "CurrentPetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Kennels_Pets_CurrentPetId1",
                table: "Kennels",
                column: "CurrentPetId1",
                principalTable: "Pets",
                principalColumn: "Id");
        }
    }
}

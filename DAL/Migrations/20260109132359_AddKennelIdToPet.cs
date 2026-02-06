using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddKennelIdToPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kennels_Pets_CurrentPetId",
                table: "Kennels");

            migrationBuilder.DropIndex(
                name: "IX_Kennels_CurrentPetId",
                table: "Kennels");

            migrationBuilder.AddColumn<Guid>(
                name: "KennelId",
                table: "Pets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentPetId1",
                table: "Kennels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_KennelId",
                table: "Pets",
                column: "KennelId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Kennels_KennelId",
                table: "Pets",
                column: "KennelId",
                principalTable: "Kennels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kennels_Pets_CurrentPetId1",
                table: "Kennels");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Kennels_KennelId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_KennelId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Kennels_CurrentPetId1",
                table: "Kennels");

            migrationBuilder.DropColumn(
                name: "KennelId",
                table: "Pets");

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}

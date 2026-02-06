using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MedicationLogCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_Medications_MedicationId",
                table: "MedicationLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationLogs_Medications_MedicationId",
                table: "MedicationLogs",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_Medications_MedicationId",
                table: "MedicationLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationLogs_Medications_MedicationId",
                table: "MedicationLogs",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

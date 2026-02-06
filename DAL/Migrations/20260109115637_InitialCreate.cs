using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PremiumLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Species = table.Column<int>(type: "INTEGER", nullable: false),
                    Breed = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Aggressive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DietaryRestrictions = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedingSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FoodType = table.Column<string>(type: "TEXT", nullable: false),
                    Restrictions = table.Column<string>(type: "TEXT", nullable: false),
                    TimesPerDay = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedingSchedules_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incompatibilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetAId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetBId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RuleDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incompatibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incompatibilities_Pets_PetAId",
                        column: x => x.PetAId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incompatibilities_Pets_PetBId",
                        column: x => x.PetBId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kennels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Zone = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    AdjacentKennelIds = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentPetId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CleaningStartedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CleaningUntilUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kennels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kennels_Pets_CurrentPetId",
                        column: x => x.CurrentPetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    SpecialCondition = table.Column<string>(type: "TEXT", nullable: false),
                    Required = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhotoUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    SentToOwner = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdateType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoUpdates_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    KennelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CleaningUntil = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stays_Kennels_KennelId",
                        column: x => x.KennelId,
                        principalTable: "Kennels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stays_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MedicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ScheduledTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Given = table.Column<bool>(type: "INTEGER", nullable: false),
                    GivenAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StaffInitials = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationLogs_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedingSchedules_PetId",
                table: "FeedingSchedules",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Incompatibilities_PetAId",
                table: "Incompatibilities",
                column: "PetAId");

            migrationBuilder.CreateIndex(
                name: "IX_Incompatibilities_PetBId",
                table: "Incompatibilities",
                column: "PetBId");

            migrationBuilder.CreateIndex(
                name: "IX_Kennels_CurrentPetId",
                table: "Kennels",
                column: "CurrentPetId");

            migrationBuilder.CreateIndex(
                name: "IX_Kennels_Name",
                table: "Kennels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicationLogs_MedicationId",
                table: "MedicationLogs",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PetId",
                table: "Medications",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_Email",
                table: "Owners",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Name",
                table: "Pets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_OwnerId",
                table: "Pets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoUpdates_PetId",
                table: "PhotoUpdates",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Stays_KennelId",
                table: "Stays",
                column: "KennelId");

            migrationBuilder.CreateIndex(
                name: "IX_Stays_PetId",
                table: "Stays",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedingSchedules");

            migrationBuilder.DropTable(
                name: "Incompatibilities");

            migrationBuilder.DropTable(
                name: "MedicationLogs");

            migrationBuilder.DropTable(
                name: "PhotoUpdates");

            migrationBuilder.DropTable(
                name: "Stays");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Kennels");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}

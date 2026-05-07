using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportFederationsAccounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_TemplatesAndMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccreditationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FederationStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederationStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SportTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Federations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Code = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    SportTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccreditationStatusId = table.Column<int>(type: "INTEGER", nullable: true),
                    FederationStateId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccreditationEndDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    LegalAddress = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PostalAddress = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    INN = table.Column<string>(type: "TEXT", maxLength: 12, nullable: true),
                    OGRN = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Federations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Federations_AccreditationStatuses_AccreditationStatusId",
                        column: x => x.AccreditationStatusId,
                        principalTable: "AccreditationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Federations_FederationStates_FederationStateId",
                        column: x => x.FederationStateId,
                        principalTable: "FederationStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Federations_SportTypes_SportTypeId",
                        column: x => x.SportTypeId,
                        principalTable: "SportTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ContactPersons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FederationId = table.Column<int>(type: "INTEGER", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Position = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactPersons_Federations_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_FederationId",
                table: "ContactPersons",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_Federations_AccreditationStatusId",
                table: "Federations",
                column: "AccreditationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Federations_Code",
                table: "Federations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Federations_FederationStateId",
                table: "Federations",
                column: "FederationStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Federations_SportTypeId",
                table: "Federations",
                column: "SportTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactPersons");

            migrationBuilder.DropTable(
                name: "Federations");

            migrationBuilder.DropTable(
                name: "AccreditationStatuses");

            migrationBuilder.DropTable(
                name: "FederationStates");

            migrationBuilder.DropTable(
                name: "SportTypes");
        }
    }
}

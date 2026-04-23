using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportFederationsAccounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeAllFederationRelationsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Federations_FederationStates_FederationStateId",
                table: "Federations");

            migrationBuilder.AlterColumn<int>(
                name: "FederationStateId",
                table: "Federations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Federations_FederationStates_FederationStateId",
                table: "Federations",
                column: "FederationStateId",
                principalTable: "FederationStates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Federations_FederationStates_FederationStateId",
                table: "Federations");

            migrationBuilder.AlterColumn<int>(
                name: "FederationStateId",
                table: "Federations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Federations_FederationStates_FederationStateId",
                table: "Federations",
                column: "FederationStateId",
                principalTable: "FederationStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

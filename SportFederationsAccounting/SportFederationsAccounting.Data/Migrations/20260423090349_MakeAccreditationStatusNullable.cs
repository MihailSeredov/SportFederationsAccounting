using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportFederationsAccounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeAccreditationStatusNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Federations_AccreditationStatuses_AccreditationStatusId",
                table: "Federations");

            migrationBuilder.AlterColumn<string>(
                name: "PostalAddress",
                table: "Federations",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Federations",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "OGRN",
                table: "Federations",
                type: "TEXT",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "LegalAddress",
                table: "Federations",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "INN",
                table: "Federations",
                type: "TEXT",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Federations",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "AccreditationStatusId",
                table: "Federations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Federations_AccreditationStatuses_AccreditationStatusId",
                table: "Federations",
                column: "AccreditationStatusId",
                principalTable: "AccreditationStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Federations_AccreditationStatuses_AccreditationStatusId",
                table: "Federations");

            migrationBuilder.AlterColumn<string>(
                name: "PostalAddress",
                table: "Federations",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Federations",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OGRN",
                table: "Federations",
                type: "TEXT",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalAddress",
                table: "Federations",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "INN",
                table: "Federations",
                type: "TEXT",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Federations",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccreditationStatusId",
                table: "Federations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Federations_AccreditationStatuses_AccreditationStatusId",
                table: "Federations",
                column: "AccreditationStatusId",
                principalTable: "AccreditationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

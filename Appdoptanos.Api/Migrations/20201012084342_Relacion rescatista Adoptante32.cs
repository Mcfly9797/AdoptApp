using Microsoft.EntityFrameworkCore.Migrations;

namespace Appdoptanos.Api.Migrations
{
    public partial class RelacionrescatistaAdoptante32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adopcion_AnimalId",
                table: "Adopcion");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "User",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "User",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reputacion",
                table: "User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreEspecie",
                table: "Especie",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Animal",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Animal",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Adopcion_AnimalId",
                table: "Adopcion",
                column: "AnimalId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adopcion_AnimalId",
                table: "Adopcion");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Reputacion",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Animal");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NombreEspecie",
                table: "Especie",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Animal",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 25);

            migrationBuilder.CreateIndex(
                name: "IX_Adopcion_AnimalId",
                table: "Adopcion",
                column: "AnimalId");
        }
    }
}

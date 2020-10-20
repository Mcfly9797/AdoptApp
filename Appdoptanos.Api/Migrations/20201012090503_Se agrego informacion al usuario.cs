using Microsoft.EntityFrameworkCore.Migrations;

namespace Appdoptanos.Api.Migrations
{
    public partial class Seagregoinformacionalusuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "User",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "User",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Domicilio",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidad",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Domicilio",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Localidad",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "User",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}

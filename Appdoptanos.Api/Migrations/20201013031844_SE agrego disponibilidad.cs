using Microsoft.EntityFrameworkCore.Migrations;

namespace Appdoptanos.Api.Migrations
{
    public partial class SEagregodisponibilidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponibilidad",
                table: "Animal",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponibilidad",
                table: "Animal");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Appdoptanos.Api.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especie",
                columns: table => new
                {
                    IdEspecie = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEspecie = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especie", x => x.IdEspecie);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUser = table.Column<string>(nullable: false),
                    Clave = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Apellido = table.Column<string>(nullable: false),
                    Dni = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    IdAnimal = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    FecNac = table.Column<DateTime>(nullable: false),
                    EspecieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.IdAnimal);
                    table.ForeignKey(
                        name: "FK_Animal_Especie_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especie",
                        principalColumn: "IdEspecie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_EspecieId",
                table: "Animal",
                column: "EspecieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Especie");
        }
    }
}

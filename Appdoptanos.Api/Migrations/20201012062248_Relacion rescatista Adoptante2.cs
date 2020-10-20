using Microsoft.EntityFrameworkCore.Migrations;

namespace Appdoptanos.Api.Migrations
{
    public partial class RelacionrescatistaAdoptante2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adopcion",
                columns: table => new
                {
                    IdAdopcion = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(nullable: false),
                    RescatistaID = table.Column<int>(nullable: true),
                    AdoptanteID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adopcion", x => x.IdAdopcion);
                    table.ForeignKey(
                        name: "FK_Adopcion_User_AdoptanteID",
                        column: x => x.AdoptanteID,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Adopcion_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animal",
                        principalColumn: "IdAnimal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adopcion_User_RescatistaID",
                        column: x => x.RescatistaID,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adopcion_AdoptanteID",
                table: "Adopcion",
                column: "AdoptanteID");

            migrationBuilder.CreateIndex(
                name: "IX_Adopcion_AnimalId",
                table: "Adopcion",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Adopcion_RescatistaID",
                table: "Adopcion",
                column: "RescatistaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adopcion");
        }
    }
}

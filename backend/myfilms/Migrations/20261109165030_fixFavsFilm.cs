using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myfilms.Migrations
{
    /// <inheritdoc />
    public partial class fixFavsFilm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filmes_Clients_ClientId",
                table: "Filmes");

            migrationBuilder.DropIndex(
                name: "IX_Filmes_ClientId",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Filmes");

            migrationBuilder.CreateTable(
                name: "ClientFilme",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "integer", nullable: false),
                    FavoriteFilmesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFilme", x => new { x.ClientsId, x.FavoriteFilmesId });
                    table.ForeignKey(
                        name: "FK_ClientFilme_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFilme_Filmes_FavoriteFilmesId",
                        column: x => x.FavoriteFilmesId,
                        principalTable: "Filmes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFilme_FavoriteFilmesId",
                table: "ClientFilme",
                column: "FavoriteFilmesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientFilme");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Filmes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Filmes_ClientId",
                table: "Filmes",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filmes_Clients_ClientId",
                table: "Filmes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}

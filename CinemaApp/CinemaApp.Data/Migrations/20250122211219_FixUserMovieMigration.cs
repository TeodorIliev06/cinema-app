using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserMovieMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserMovie_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserMovie_Movies_MovieId",
                table: "ApplicationUserMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserMovie",
                table: "ApplicationUserMovie");

            migrationBuilder.RenameTable(
                name: "ApplicationUserMovie",
                newName: "UsersMovies");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserMovie_MovieId",
                table: "UsersMovies",
                newName: "IX_UsersMovies_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersMovies",
                table: "UsersMovies",
                columns: new[] { "ApplicationUserId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersMovies_AspNetUsers_ApplicationUserId",
                table: "UsersMovies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersMovies_Movies_MovieId",
                table: "UsersMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersMovies_AspNetUsers_ApplicationUserId",
                table: "UsersMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersMovies_Movies_MovieId",
                table: "UsersMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersMovies",
                table: "UsersMovies");

            migrationBuilder.RenameTable(
                name: "UsersMovies",
                newName: "ApplicationUserMovie");

            migrationBuilder.RenameIndex(
                name: "IX_UsersMovies_MovieId",
                table: "ApplicationUserMovie",
                newName: "IX_ApplicationUserMovie_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserMovie",
                table: "ApplicationUserMovie",
                columns: new[] { "ApplicationUserId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserMovie_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserMovie",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserMovie_Movies_MovieId",
                table: "ApplicationUserMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

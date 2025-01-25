using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeModelsSoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cinemas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("9a67f8cb-8b3c-4f53-9429-d7b4c912f3b1"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("ae3b4c7d-3b95-4a84-a6b3-18c8b4d8f45d"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("c4e8c967-b54f-4c53-96b8-f8bcd6e9c4c5"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("d8346e8e-392c-4872-9c9e-d3c6e8e5b784"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("fb4c8e3d-4a53-4c67-95b9-e3d4c912f8a7"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("4a8c9b3e-7f8c-45c6-8d9b-c3e7f9b4a6d8"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("6b12f8a7-d4c8-41b9-8f3d-9c6e7d8f45c2"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("8c9e3d67-4a53-4b95-b8f3-6c7d8f4a9c5e"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("9c8b7e45-df8c-412c-9b3e-7d8f4b6c9f3d"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("f8b4c67d-d912-4e5c-8b3c-6e7d45c3b9f4"),
                column: "IsDeleted",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cinemas");
        }
    }
}

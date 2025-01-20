using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class PreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("69a42c94-bdfb-430a-8caf-35a23d17e099"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("fca6473b-091d-4a4e-855e-0029539020b5"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("9416243e-cd80-4825-9c7e-6e4b2a015e7c"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("9550f1c9-56a2-4bb7-abbd-c2bc510f826e"));

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("9a67f8cb-8b3c-4f53-9429-d7b4c912f3b1"), "456 Sunset Blvd, Los Angeles", "Sunset Boulevard Theater" },
                    { new Guid("ae3b4c7d-3b95-4a84-a6b3-18c8b4d8f45d"), "123 Main Street, New York", "Grand Central Cinema" },
                    { new Guid("c4e8c967-b54f-4c53-96b8-f8bcd6e9c4c5"), "101 Coastal Road, Miami", "Oceanview Cinema" },
                    { new Guid("d8346e8e-392c-4872-9c9e-d3c6e8e5b784"), "789 Broadway, Chicago", "Downtown Cineplex" },
                    { new Guid("fb4c8e3d-4a53-4c67-95b9-e3d4c912f8a7"), "202 Maple Avenue, Toronto", "Maple Leaf Theater" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("4a8c9b3e-7f8c-45c6-8d9b-c3e7f9b4a6d8"), "A heartwarming story of love and resilience.", "Michael Brown", 105, "Romance", new DateTime(2021, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dancing in the Rain" },
                    { new Guid("6b12f8a7-d4c8-41b9-8f3d-9c6e7d8f45c2"), "An epic journey through uncharted territories.", "John Smith", 120, "Adventure", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Great Adventure" },
                    { new Guid("8c9e3d67-4a53-4b95-b8f3-6c7d8f4a9c5e"), "A hilarious take on everyday life.", "Jane Doe", 90, "Comedy", new DateTime(2022, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laugh Out Loud" },
                    { new Guid("9c8b7e45-df8c-412c-9b3e-7d8f4b6c9f3d"), "A thought-provoking journey through a dystopian future.", "Sophia Cheng", 115, "Science Fiction", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Future Shock" },
                    { new Guid("f8b4c67d-d912-4e5c-8b3c-6e7d45c3b9f4"), "A suspenseful thriller unraveling secrets of the deep sea.", "Chris Lee", 140, "Mystery", new DateTime(2024, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mystery of the Depths" }
                });

            migrationBuilder.InsertData(
                table: "CinemasMovies",
                columns: new[] { "CinemaId", "MovieId" },
                values: new object[,]
                {
                    { new Guid("9a67f8cb-8b3c-4f53-9429-d7b4c912f3b1"), new Guid("8c9e3d67-4a53-4b95-b8f3-6c7d8f4a9c5e") },
                    { new Guid("ae3b4c7d-3b95-4a84-a6b3-18c8b4d8f45d"), new Guid("6b12f8a7-d4c8-41b9-8f3d-9c6e7d8f45c2") },
                    { new Guid("c4e8c967-b54f-4c53-96b8-f8bcd6e9c4c5"), new Guid("9c8b7e45-df8c-412c-9b3e-7d8f4b6c9f3d") },
                    { new Guid("d8346e8e-392c-4872-9c9e-d3c6e8e5b784"), new Guid("f8b4c67d-d912-4e5c-8b3c-6e7d45c3b9f4") },
                    { new Guid("fb4c8e3d-4a53-4c67-95b9-e3d4c912f8a7"), new Guid("4a8c9b3e-7f8c-45c6-8d9b-c3e7f9b4a6d8") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CinemasMovies",
                keyColumns: new[] { "CinemaId", "MovieId" },
                keyValues: new object[] { new Guid("9a67f8cb-8b3c-4f53-9429-d7b4c912f3b1"), new Guid("8c9e3d67-4a53-4b95-b8f3-6c7d8f4a9c5e") });

            migrationBuilder.DeleteData(
                table: "CinemasMovies",
                keyColumns: new[] { "CinemaId", "MovieId" },
                keyValues: new object[] { new Guid("ae3b4c7d-3b95-4a84-a6b3-18c8b4d8f45d"), new Guid("6b12f8a7-d4c8-41b9-8f3d-9c6e7d8f45c2") });

            migrationBuilder.DeleteData(
                table: "CinemasMovies",
                keyColumns: new[] { "CinemaId", "MovieId" },
                keyValues: new object[] { new Guid("c4e8c967-b54f-4c53-96b8-f8bcd6e9c4c5"), new Guid("9c8b7e45-df8c-412c-9b3e-7d8f4b6c9f3d") });

            migrationBuilder.DeleteData(
                table: "CinemasMovies",
                keyColumns: new[] { "CinemaId", "MovieId" },
                keyValues: new object[] { new Guid("d8346e8e-392c-4872-9c9e-d3c6e8e5b784"), new Guid("f8b4c67d-d912-4e5c-8b3c-6e7d45c3b9f4") });

            migrationBuilder.DeleteData(
                table: "CinemasMovies",
                keyColumns: new[] { "CinemaId", "MovieId" },
                keyValues: new object[] { new Guid("fb4c8e3d-4a53-4c67-95b9-e3d4c912f8a7"), new Guid("4a8c9b3e-7f8c-45c6-8d9b-c3e7f9b4a6d8") });

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("9a67f8cb-8b3c-4f53-9429-d7b4c912f3b1"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("ae3b4c7d-3b95-4a84-a6b3-18c8b4d8f45d"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("c4e8c967-b54f-4c53-96b8-f8bcd6e9c4c5"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("d8346e8e-392c-4872-9c9e-d3c6e8e5b784"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("fb4c8e3d-4a53-4c67-95b9-e3d4c912f8a7"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("4a8c9b3e-7f8c-45c6-8d9b-c3e7f9b4a6d8"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("6b12f8a7-d4c8-41b9-8f3d-9c6e7d8f45c2"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("8c9e3d67-4a53-4b95-b8f3-6c7d8f4a9c5e"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("9c8b7e45-df8c-412c-9b3e-7d8f4b6c9f3d"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("f8b4c67d-d912-4e5c-8b3c-6e7d45c3b9f4"));

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("69a42c94-bdfb-430a-8caf-35a23d17e099"), "456 Elm Street, Metropolis", "Galaxy Theaters" },
                    { new Guid("fca6473b-091d-4a4e-855e-0029539020b5"), "123 Main Street, Springfield", "Starlight Cinema" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("9416243e-cd80-4825-9c7e-6e4b2a015e7c"), "When the menace known as The Joker emerges from his mysterious past, he wreaks havoc and chaos on the people of Gotham.", "Christopher Nolan", 152, "Action", new DateTime(2008, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Dark Knight" },
                    { new Guid("9550f1c9-56a2-4bb7-abbd-c2bc510f826e"), "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO.", "Christopher Nolan", 148, "Sci-Fi", new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inception" }
                });
        }
    }
}

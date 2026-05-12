using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lumina.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingScreeningInConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Screenings",
                columns: new[] { "Id", "AvailableTickets", "CinemaId", "MovieId", "Showtime" },
                values: new object[,]
                {
                    { new Guid("1aa3bdcd-202b-43dc-9979-44a847f37359"), 120, new Guid("30c8c49e-647d-4a5c-ba64-012263bd0ae5"), new Guid("e00208b1-cb12-4bd4-8ac1-36ab62f7b4c9"), "21:00" },
                    { new Guid("8e1b72e6-40c7-4525-bf51-3eb2116e1040"), 150, new Guid("30c8c49e-647d-4a5c-ba64-012263bd0ae5"), new Guid("c9850e58-01c5-4263-c422-08de3500570b"), "18:00" },
                    { new Guid("9024bb4f-f351-45d6-9587-53c739435960"), 180, new Guid("30c8c49e-647d-4a5c-ba64-012263bd0ae5"), new Guid("ae50a5ab-9642-466f-b528-3cc61071bb4c"), "19:30" },
                    { new Guid("c27ca254-597a-4dfd-9d9a-d09d11960769"), 90, new Guid("30c8c49e-647d-4a5c-ba64-012263bd0ae5"), new Guid("54082f99-023b-4d68-89ac-44c00a0958d0"), "16:45" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: new Guid("1aa3bdcd-202b-43dc-9979-44a847f37359"));

            migrationBuilder.DeleteData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: new Guid("8e1b72e6-40c7-4525-bf51-3eb2116e1040"));

            migrationBuilder.DeleteData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: new Guid("9024bb4f-f351-45d6-9587-53c739435960"));

            migrationBuilder.DeleteData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: new Guid("c27ca254-597a-4dfd-9d9a-d09d11960769"));
        }
    }
}

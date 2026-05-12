using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumina.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedOneScreeningMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Screenings",
                columns: new[] { "Id", "AvailableTickets", "CinemaId", "MovieId", "Showtime" },
                values: new object[] { new Guid("fd910bcc-d507-413d-8a45-91375537e2a2"), 100, new Guid("30c8c49e-647d-4a5c-ba64-012263bd0ae5"), new Guid("c9850e58-01c5-4263-c422-08de3500570b"), "20:00" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Screenings",
                keyColumn: "Id",
                keyValue: new Guid("fd910bcc-d507-413d-8a45-91375537e2a2"));
        }
    }
}

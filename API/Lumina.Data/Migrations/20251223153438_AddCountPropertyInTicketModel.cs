using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumina.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCountPropertyInTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Tickets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rentals.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyToRentableVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "RentableVehicles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "RentableVehicles");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rentals.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Rentals",
                newName: "CustomerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Rentals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Rentals");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Rentals",
                newName: "CustomerName");
        }
    }
}

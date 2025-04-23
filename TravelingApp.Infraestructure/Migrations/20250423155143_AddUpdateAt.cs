using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelingApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "Travel",
                table: "Users",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                schema: "Travel",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "Travel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                schema: "Travel",
                table: "Users");
        }
    }
}

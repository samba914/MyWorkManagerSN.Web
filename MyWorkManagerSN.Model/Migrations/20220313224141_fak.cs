using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWorkManagerSN.Model.Migrations
{
    public partial class fak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreationInvoice",
                table: "Order",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreationInvoice",
                table: "Order");
        }
    }
}

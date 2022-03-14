using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWorkManagerSN.Model.Migrations
{
    public partial class ak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountTotalHT",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountTotalHT",
                table: "Order");
        }
    }
}

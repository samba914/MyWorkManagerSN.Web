using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWorkManagerSN.Model.Migrations
{
    public partial class discout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Order",
                newName: "DiscountTTC");

            migrationBuilder.AddColumn<double>(
                name: "DiscountHT",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountTVA",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountHT",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DiscountTVA",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "DiscountTTC",
                table: "Order",
                newName: "Discount");
        }
    }
}

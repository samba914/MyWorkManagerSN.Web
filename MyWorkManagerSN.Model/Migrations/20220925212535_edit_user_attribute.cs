using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWorkManagerSN.Model.Migrations
{
    public partial class edit_user_attribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HaveActiveContract",
                table: "User",
                newName: "HaveActiveContractOrTrial");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HaveActiveContractOrTrial",
                table: "User",
                newName: "HaveActiveContract");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWorkManagerSN.Model.Migrations
{
    public partial class contract_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ContractId",
                table: "User",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Contracts_ContractId",
                table: "User",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Contracts_ContractId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ContractId",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

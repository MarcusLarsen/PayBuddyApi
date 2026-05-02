using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayBuddyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDebtStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Debts");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Debts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Debts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Debts");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Debts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Debts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

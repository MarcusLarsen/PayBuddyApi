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
        }
    }
}

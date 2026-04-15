using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayBuddyApi.Migrations
{
    /// <inheritdoc />
    public partial class CreatedDebt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Debts",
                newName: "DebtId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DebtId",
                table: "Debts",
                newName: "Id");
        }
    }
}

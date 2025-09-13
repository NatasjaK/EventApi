using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentsApi.Migrations
{
    /// <inheritdoc />
    public partial class Payments_SetDecimalPrecisionAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Date",
                table: "Transactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_EventId",
                table: "Transactions",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Date",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_EventId",
                table: "Transactions");
        }
    }
}

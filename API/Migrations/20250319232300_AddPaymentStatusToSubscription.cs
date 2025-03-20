using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatusToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Subscriptions_SubscriptionId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SubscriptionId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Subscriptions",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "Invoices",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Invoices",
                newName: "ClientEmail");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Invoices",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Subscriptions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Invoices",
                newName: "PurchaseDate");

            migrationBuilder.RenameColumn(
                name: "ClientEmail",
                table: "Invoices",
                newName: "Email");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubscriptionId",
                table: "Invoices",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Subscriptions_SubscriptionId",
                table: "Invoices",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

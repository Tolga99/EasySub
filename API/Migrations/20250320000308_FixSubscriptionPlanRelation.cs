using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class FixSubscriptionPlanRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "DurationMonths",
                table: "Subscriptions",
                newName: "SubscriptionPlanId");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionTypeId = table.Column<int>(type: "int", nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPlans_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionPlans_SubscriptionType_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriptionPlanId",
                table: "Subscriptions",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubscriptionId",
                table: "Invoices",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubscriptionPlanId",
                table: "Invoices",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_BrandId",
                table: "SubscriptionPlans",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_SubscriptionTypeId",
                table: "SubscriptionPlans",
                column: "SubscriptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SubscriptionPlans_SubscriptionPlanId",
                table: "Invoices",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Subscriptions_SubscriptionId",
                table: "Invoices",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "Subscriptions",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SubscriptionPlans_SubscriptionPlanId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Subscriptions_SubscriptionId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "SubscriptionType");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SubscriptionPlanId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SubscriptionId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SubscriptionPlanId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "SubscriptionPlanId",
                table: "Subscriptions",
                newName: "DurationMonths");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionPlans_Brand_BrandId",
                table: "SubscriptionPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionPlans_SubscriptionType_SubscriptionTypeId",
                table: "SubscriptionPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionType",
                table: "SubscriptionType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.RenameTable(
                name: "SubscriptionType",
                newName: "SubscriptionTypes");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionTypes",
                table: "SubscriptionTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BrandSubscriptionTypes",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionTypeId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandSubscriptionTypes", x => new { x.BrandId, x.SubscriptionTypeId });
                    table.ForeignKey(
                        name: "FK_BrandSubscriptionTypes_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandSubscriptionTypes_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandSubscriptionTypes_SubscriptionTypeId",
                table: "BrandSubscriptionTypes",
                column: "SubscriptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionPlans_Brands_BrandId",
                table: "SubscriptionPlans",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionPlans_SubscriptionTypes_SubscriptionTypeId",
                table: "SubscriptionPlans",
                column: "SubscriptionTypeId",
                principalTable: "SubscriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionPlans_Brands_BrandId",
                table: "SubscriptionPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionPlans_SubscriptionTypes_SubscriptionTypeId",
                table: "SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "BrandSubscriptionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionTypes",
                table: "SubscriptionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "SubscriptionTypes",
                newName: "SubscriptionType");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionType",
                table: "SubscriptionType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionPlans_Brand_BrandId",
                table: "SubscriptionPlans",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionPlans_SubscriptionType_SubscriptionTypeId",
                table: "SubscriptionPlans",
                column: "SubscriptionTypeId",
                principalTable: "SubscriptionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

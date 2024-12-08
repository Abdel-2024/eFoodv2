using Microsoft.EntityFrameworkCore.Migrations;

namespace eFood.Persistence.Migrations
{
    public partial class completerProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeOption_TypeOption",
                table: "TypeOption");

            migrationBuilder.DropIndex(
                name: "IX_TypeOption_ParentId",
                table: "TypeOption");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "TypeOption");

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommended",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsRecommended",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "TypeOption",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeOption_ParentId",
                table: "TypeOption",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeOption_TypeOption",
                table: "TypeOption",
                column: "ParentId",
                principalTable: "TypeOption",
                principalColumn: "TypeOptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MyShop.EntityFrameworkCore.DbMigration.Migrations
{
    public partial class modifyProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Product",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Product");
        }
    }
}

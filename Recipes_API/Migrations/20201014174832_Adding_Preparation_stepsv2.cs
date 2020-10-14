using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes_API.Migrations
{
    public partial class Adding_Preparation_stepsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preparation",
                table: "Recipes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Preparation",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

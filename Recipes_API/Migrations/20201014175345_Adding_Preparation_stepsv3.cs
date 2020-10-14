using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes_API.Migrations
{
    public partial class Adding_Preparation_stepsv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreparationSteps_Recipes_RecipeId",
                table: "PreparationSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreparationSteps",
                table: "PreparationSteps");

            migrationBuilder.RenameTable(
                name: "PreparationSteps",
                newName: "PreparationStepses");

            migrationBuilder.RenameIndex(
                name: "IX_PreparationSteps_RecipeId",
                table: "PreparationStepses",
                newName: "IX_PreparationStepses_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreparationStepses",
                table: "PreparationStepses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreparationStepses_Recipes_RecipeId",
                table: "PreparationStepses",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreparationStepses_Recipes_RecipeId",
                table: "PreparationStepses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreparationStepses",
                table: "PreparationStepses");

            migrationBuilder.RenameTable(
                name: "PreparationStepses",
                newName: "PreparationSteps");

            migrationBuilder.RenameIndex(
                name: "IX_PreparationStepses_RecipeId",
                table: "PreparationSteps",
                newName: "IX_PreparationSteps_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreparationSteps",
                table: "PreparationSteps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreparationSteps_Recipes_RecipeId",
                table: "PreparationSteps",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

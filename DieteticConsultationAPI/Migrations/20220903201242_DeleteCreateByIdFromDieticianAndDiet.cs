using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DieteticConsultationAPI.Migrations
{
    public partial class DeleteCreateByIdFromDieticianAndDiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dieticians_Users_CreatedById",
                table: "Dieticians");

            migrationBuilder.DropForeignKey(
                name: "FK_Diets_Users_CreatedById",
                table: "Diets");

            migrationBuilder.DropIndex(
                name: "IX_Diets_CreatedById",
                table: "Diets");

            migrationBuilder.DropIndex(
                name: "IX_Dieticians_CreatedById",
                table: "Dieticians");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Diets");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Dieticians");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Diets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Dieticians",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diets_CreatedById",
                table: "Diets",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Dieticians_CreatedById",
                table: "Dieticians",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Dieticians_Users_CreatedById",
                table: "Dieticians",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diets_Users_CreatedById",
                table: "Diets",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DieteticConsultationAPI.Migrations
{
    public partial class AddedNullablePatientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diets_Patients_PatientId",
                table: "Diets");

            migrationBuilder.DropIndex(
                name: "IX_Diets_PatientId",
                table: "Diets");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Diets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Diets_PatientId",
                table: "Diets",
                column: "PatientId",
                unique: true,
                filter: "[PatientId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Diets_Patients_PatientId",
                table: "Diets",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diets_Patients_PatientId",
                table: "Diets");

            migrationBuilder.DropIndex(
                name: "IX_Diets_PatientId",
                table: "Diets");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Diets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diets_PatientId",
                table: "Diets",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Diets_Patients_PatientId",
                table: "Diets",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

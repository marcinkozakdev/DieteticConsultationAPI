using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DieteticConsultationAPI.Migrations
{
    public partial class AddedNullableInPatientModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Dieticians_DieticianId",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "DieticianId",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Dieticians_DieticianId",
                table: "Patients",
                column: "DieticianId",
                principalTable: "Dieticians",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Dieticians_DieticianId",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "DieticianId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Dieticians_DieticianId",
                table: "Patients",
                column: "DieticianId",
                principalTable: "Dieticians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

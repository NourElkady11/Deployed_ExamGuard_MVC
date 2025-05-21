using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.Data
{
    /// <inheritdoc />
    public partial class EditDeleteExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheatingReports_Exams_ExamId",
                table: "CheatingReports");

            migrationBuilder.AddForeignKey(
                name: "FK_CheatingReports_Exams_ExamId",
                table: "CheatingReports",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheatingReports_Exams_ExamId",
                table: "CheatingReports");

            migrationBuilder.AddForeignKey(
                name: "FK_CheatingReports_Exams_ExamId",
                table: "CheatingReports",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.Data
{
    /// <inheritdoc />
    public partial class EditDeleteExamsFinalIsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Students_StudentId",
                table: "studentAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Students_StudentId",
                table: "studentAnswers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Students_StudentId",
                table: "studentAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Students_StudentId",
                table: "studentAnswers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}

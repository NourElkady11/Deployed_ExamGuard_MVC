using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.Data
{
    /// <inheritdoc />
    public partial class EditDeleteExamsFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choice_Question_QuestionId",
                table: "Choice");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Choice_ChoiceId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Question_QuestionId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Students_StudentId",
                table: "studentAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_Choice_Question_QuestionId",
                table: "Choice",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Choice_ChoiceId",
                table: "studentAnswers",
                column: "ChoiceId",
                principalTable: "Choice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Question_QuestionId",
                table: "studentAnswers",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Students_StudentId",
                table: "studentAnswers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choice_Question_QuestionId",
                table: "Choice");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Choice_ChoiceId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Question_QuestionId",
                table: "studentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_studentAnswers_Students_StudentId",
                table: "studentAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_Choice_Question_QuestionId",
                table: "Choice",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Choice_ChoiceId",
                table: "studentAnswers",
                column: "ChoiceId",
                principalTable: "Choice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Exams_ExamId",
                table: "studentAnswers",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentAnswers_Question_QuestionId",
                table: "studentAnswers",
                column: "QuestionId",
                principalTable: "Question",
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
    }
}

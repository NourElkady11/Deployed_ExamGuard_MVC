using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.Data
{
    /// <inheritdoc />
    public partial class CourseStudentRemovingGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "CourseStudents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Grade",
                table: "CourseStudents",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

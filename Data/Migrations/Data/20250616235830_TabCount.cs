using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.Data
{
    /// <inheritdoc />
    public partial class TabCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TabSwitchCount",
                table: "CheatingReports",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TabSwitchCount",
                table: "CheatingReports");
        }
    }
}

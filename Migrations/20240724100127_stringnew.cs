using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAndCourses.Migrations
{
    /// <inheritdoc />
    public partial class stringnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvailablesONString",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchesString",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailablesONString",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "BranchesString",
                table: "Colleges");
        }
    }
}

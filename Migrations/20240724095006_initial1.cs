using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeAndCourses.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colleges",
                columns: table => new
                {
                    CollegeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreferredContactMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNAACAccrediet = table.Column<bool>(type: "bit", nullable: false),
                    RecevieNewsLetter = table.Column<bool>(type: "bit", nullable: false),
                    HowManyStudents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNo = table.Column<int>(type: "int", nullable: false),
                    Branches = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleges", x => x.CollegeId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fees = table.Column<int>(type: "int", nullable: false),
                    DurationOfCourse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollegeId = table.Column<int>(type: "int", nullable: false),
                    Publish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoureseFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicationAccrediet = table.Column<bool>(type: "bit", nullable: false),
                    FreeCourese = table.Column<bool>(type: "bit", nullable: false),
                    BooksForCourse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BracodeOfBook = table.Column<int>(type: "int", nullable: false),
                    AvailablesON = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "CollegeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CollegeId",
                table: "Courses",
                column: "CollegeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Colleges");
        }
    }
}

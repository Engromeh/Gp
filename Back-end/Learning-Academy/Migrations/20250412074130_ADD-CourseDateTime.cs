using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Academy.Migrations
{
    /// <inheritdoc />
    public partial class ADDCourseDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDuration",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "CourseDateTime",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDateTime",
                table: "Courses");

            migrationBuilder.AddColumn<DateTime>(
                name: "CourseDuration",
                table: "Courses",
                type: "datetime2",
                nullable: true);
        }
    }
}

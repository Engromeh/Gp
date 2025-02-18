using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Academy.Migrations
{
    /// <inheritdoc />
    public partial class MakeCertificateIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Certificate_CertificateId",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "CertificateId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Certificate_CertificateId",
                table: "Courses",
                column: "CertificateId",
                principalTable: "Certificate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Certificate_CertificateId",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "CertificateId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Certificate_CertificateId",
                table: "Courses",
                column: "CertificateId",
                principalTable: "Certificate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

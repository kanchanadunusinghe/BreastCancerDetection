using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCD.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BCD0005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MammographyScans_Patients_PatientId1",
                table: "MammographyScans");

            migrationBuilder.DropIndex(
                name: "IX_MammographyScans_PatientId1",
                table: "MammographyScans");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "MammographyScans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId1",
                table: "MammographyScans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MammographyScans_PatientId1",
                table: "MammographyScans",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MammographyScans_Patients_PatientId1",
                table: "MammographyScans",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}

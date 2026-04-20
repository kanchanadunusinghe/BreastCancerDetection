using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCD.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BCD0004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "MammographyScans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "MammographyScans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MammographyScans_CreatedUserId",
                table: "MammographyScans",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MammographyScans_Users_CreatedUserId",
                table: "MammographyScans",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MammographyScans_Users_CreatedUserId",
                table: "MammographyScans");

            migrationBuilder.DropIndex(
                name: "IX_MammographyScans_CreatedUserId",
                table: "MammographyScans");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "MammographyScans");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "MammographyScans");
        }
    }
}

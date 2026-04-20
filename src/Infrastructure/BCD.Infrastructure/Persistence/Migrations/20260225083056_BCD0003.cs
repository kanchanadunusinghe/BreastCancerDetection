using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCD.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BCD0003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patients");
        }
    }
}

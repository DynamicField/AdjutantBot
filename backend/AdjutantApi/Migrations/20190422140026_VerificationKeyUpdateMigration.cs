using Microsoft.EntityFrameworkCore.Migrations;

namespace AdjutantApi.Migrations
{
    public partial class VerificationKeyUpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "VerificationKeys",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "VerificationKeys");
        }
    }
}

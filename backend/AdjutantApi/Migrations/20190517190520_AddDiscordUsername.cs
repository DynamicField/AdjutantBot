using Microsoft.EntityFrameworkCore.Migrations;

namespace AdjutantApi.Migrations
{
    public partial class AddDiscordUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordUsername",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordUsername",
                table: "AspNetUsers");
        }
    }
}

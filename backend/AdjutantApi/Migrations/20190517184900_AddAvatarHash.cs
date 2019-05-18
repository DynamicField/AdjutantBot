using Microsoft.EntityFrameworkCore.Migrations;

namespace AdjutantApi.Migrations
{
    public partial class AddAvatarHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarHash",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarHash",
                table: "AspNetUsers");
        }
    }
}

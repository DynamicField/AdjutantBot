using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AdjutantApi.Migrations
{
    public partial class AddOwnIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordAccounts");

            migrationBuilder.AddColumn<int>(
                name: "BoundKeyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BoundKeyId",
                table: "AspNetUsers",
                column: "BoundKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_VerificationKeys_BoundKeyId",
                table: "AspNetUsers",
                column: "BoundKeyId",
                principalTable: "VerificationKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_VerificationKeys_BoundKeyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BoundKeyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BoundKeyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "DiscordAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AccountId = table.Column<string>(nullable: true),
                    BoundKeyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordAccounts_VerificationKeys_BoundKeyId",
                        column: x => x.BoundKeyId,
                        principalTable: "VerificationKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordAccounts_BoundKeyId",
                table: "DiscordAccounts",
                column: "BoundKeyId");
        }
    }
}

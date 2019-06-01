using Microsoft.EntityFrameworkCore.Migrations;

namespace AdjutantApi.Migrations
{
    public partial class AddIndexesAndColumnRestrictions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KeyValue",
                table: "VerificationKeys",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiscordUsername",
                table: "AspNetUsers",
                maxLength: 37,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VerificationKeys_KeyValue",
                table: "VerificationKeys",
                column: "KeyValue",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VerificationKeys_KeyValue",
                table: "VerificationKeys");

            migrationBuilder.AlterColumn<string>(
                name: "KeyValue",
                table: "VerificationKeys",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DiscordUsername",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 37,
                oldNullable: true);
        }
    }
}

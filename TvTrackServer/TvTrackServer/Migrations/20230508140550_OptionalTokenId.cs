using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTrackServer.Migrations
{
    /// <inheritdoc />
    public partial class OptionalTokenId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tokens_TokensId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TokensId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "TokensId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TokensId",
                table: "Users",
                column: "TokensId",
                unique: true,
                filter: "[TokensId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tokens_TokensId",
                table: "Users",
                column: "TokensId",
                principalTable: "Tokens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tokens_TokensId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TokensId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "TokensId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TokensId",
                table: "Users",
                column: "TokensId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tokens_TokensId",
                table: "Users",
                column: "TokensId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

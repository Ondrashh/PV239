using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTrackServer.Migrations
{
    /// <inheritdoc />
    public partial class AddTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TokensId",
                table: "Users",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FCMDeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleCalendarToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleCalendarRefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TokensId",
                table: "Users",
                column: "TokensId",
                unique: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tokens_TokensId",
                table: "Users",
                column: "TokensId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tokens_TokensId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Users_TokensId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokensId",
                table: "Users");
        }
    }
}

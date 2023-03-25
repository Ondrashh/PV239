using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTrackServer.Migrations
{
    /// <inheritdoc />
    public partial class UserRatedBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserRating",
                table: "ShowActivities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserRated",
                table: "ShowActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRated",
                table: "ShowActivities");

            migrationBuilder.AlterColumn<int>(
                name: "UserRating",
                table: "ShowActivities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

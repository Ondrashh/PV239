using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTrackServer.Migrations
{
    /// <inheritdoc />
    public partial class ShowEpisodeActivityLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowListItems_ShowLists_ShowListId",
                table: "ShowListItems");

            migrationBuilder.AlterColumn<int>(
                name: "ShowListId",
                table: "ShowListItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShowActivityId",
                table: "EpisodeActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeActivities_ShowActivityId",
                table: "EpisodeActivities",
                column: "ShowActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EpisodeActivities_ShowActivities_ShowActivityId",
                table: "EpisodeActivities",
                column: "ShowActivityId",
                principalTable: "ShowActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShowListItems_ShowLists_ShowListId",
                table: "ShowListItems",
                column: "ShowListId",
                principalTable: "ShowLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EpisodeActivities_ShowActivities_ShowActivityId",
                table: "EpisodeActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_ShowListItems_ShowLists_ShowListId",
                table: "ShowListItems");

            migrationBuilder.DropIndex(
                name: "IX_EpisodeActivities_ShowActivityId",
                table: "EpisodeActivities");

            migrationBuilder.DropColumn(
                name: "ShowActivityId",
                table: "EpisodeActivities");

            migrationBuilder.AlterColumn<int>(
                name: "ShowListId",
                table: "ShowListItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowListItems_ShowLists_ShowListId",
                table: "ShowListItems",
                column: "ShowListId",
                principalTable: "ShowLists",
                principalColumn: "Id");
        }
    }
}

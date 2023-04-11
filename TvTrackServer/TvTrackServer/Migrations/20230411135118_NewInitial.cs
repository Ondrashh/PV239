using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTrackServer.Migrations
{
    /// <inheritdoc />
    public partial class NewInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRatedShows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TvMazeId = table.Column<int>(type: "int", nullable: false),
                    UserRatingCount = table.Column<int>(type: "int", nullable: false),
                    UserRating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatedShows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShowActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TvMazeId = table.Column<int>(type: "int", nullable: false),
                    Notifications = table.Column<bool>(type: "bit", nullable: false),
                    Calendar = table.Column<bool>(type: "bit", nullable: false),
                    UserRated = table.Column<bool>(type: "bit", nullable: false),
                    UserRating = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowActivities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Default = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpisodeActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TvMazeId = table.Column<int>(type: "int", nullable: false),
                    Watched = table.Column<bool>(type: "bit", nullable: false),
                    ShowActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpisodeActivities_ShowActivities_ShowActivityId",
                        column: x => x.ShowActivityId,
                        principalTable: "ShowActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TvMazeId = table.Column<int>(type: "int", nullable: false),
                    ShowListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowListItems_ShowLists_ShowListId",
                        column: x => x.ShowListId,
                        principalTable: "ShowLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeActivities_ShowActivityId",
                table: "EpisodeActivities",
                column: "ShowActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowActivities_UserId",
                table: "ShowActivities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowListItems_ShowListId",
                table: "ShowListItems",
                column: "ShowListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowLists_UserId",
                table: "ShowLists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpisodeActivities");

            migrationBuilder.DropTable(
                name: "ShowListItems");

            migrationBuilder.DropTable(
                name: "UserRatedShows");

            migrationBuilder.DropTable(
                name: "ShowActivities");

            migrationBuilder.DropTable(
                name: "ShowLists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

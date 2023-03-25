using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models;
using TvTrackServer.Models.Database;

namespace TvTrackServer
{
    public class TvTrackServerDbContext : DbContext
    {
        public TvTrackServerDbContext(DbContextOptions<TvTrackServerDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ShowList> ShowLists { get; set; }
        public DbSet<ShowListItem> ShowListItems { get; set; }
        public DbSet<ShowActivity> ShowActivities { get; set; }
        public DbSet<EpisodeActivity> EpisodeActivities { get; set; }
        public DbSet<UserRatedShow> UserRatedShows { get; set; }
    }
}

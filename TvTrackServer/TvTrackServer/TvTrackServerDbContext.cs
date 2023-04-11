using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models;
using TvTrackServer.Models.Database;

namespace TvTrackServer;

public class TvTrackServerDbContext : DbContext
{
    public TvTrackServerDbContext(DbContextOptions<TvTrackServerDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ShowList> ShowLists { get; set; }
    public DbSet<ShowListItem> ShowListItems { get; set; }
    public DbSet<ShowActivity> ShowActivities { get; set; }
    public DbSet<EpisodeActivity> EpisodeActivities { get; set; }
    public DbSet<UserRatedShow> UserRatedShows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.ShowLists)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(e => e.ShowActivities)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShowList>()
            .HasMany(e => e.Shows)
            .WithOne(e => e.ShowList)
            .HasForeignKey(e => e.ShowListId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShowActivity>()
            .HasMany(e => e.EpisodeActivities)
            .WithOne(e => e.ShowActivity)
            .HasForeignKey(e => e.ShowActivityId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShowActivity>().Navigation(e => e.EpisodeActivities).AutoInclude();

        base.OnModelCreating(modelBuilder);
    }
}

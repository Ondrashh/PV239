namespace TvTrackServer.Models.Database;

public class ShowActivity
{
    public int Id { get; set; }
    public int TvMazeId { get; set; }

    public bool Notifications { get; set; } = false;
    public bool Calendar { get; set; } = false;

    public bool UserRated { get; set; } = false;
    public int UserRating { get; set; } = 0;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<EpisodeActivity> EpisodeActivities = new();
}
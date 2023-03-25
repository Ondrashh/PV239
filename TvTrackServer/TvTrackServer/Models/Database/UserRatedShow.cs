namespace TvTrackServer.Models.Database;

public class UserRatedShow
{
    public int Id { get; set; }
    public int TvMazeId { get; set; }

    public int UserRatingCount { get; set; } = 0;
    public float UserRating { get; set; }
}
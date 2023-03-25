namespace TvTrackServer.Models.Database;

public class EpisodeActivity
{
    public int Id { get; set; }
    public int TvMazeId { get; set; }
    
    public bool Watched { get; set; }
}
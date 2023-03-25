namespace TvTrackServer.Models.Database
{
    /// <summary>
    /// Entity holding TvMazeId identification for listing purposes.
    /// </summary>
    public class ShowListItem
    {
        public int Id { get; set; }
        public int TvMazeId { get; set; }
        public int ShowListId { get; set; }
    }
}

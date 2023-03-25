namespace TvTrackServer.Models.Database;

public class ShowList
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Default { get; set; } = false;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public IList<ShowListItem> Shows { get; set; } = new List<ShowListItem>();
}

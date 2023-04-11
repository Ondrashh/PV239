using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvTrackServer.Models.Database;

public class ShowActivity
{
    [Key]
    public int Id { get; set; }
    public int TvMazeId { get; set; }

    public bool Notifications { get; set; } = false;
    public bool Calendar { get; set; } = false;

    public bool UserRated { get; set; } = false;
    public int UserRating { get; set; } = 0;


    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [InverseProperty(nameof(EpisodeActivity.ShowActivity))]
    public List<EpisodeActivity> EpisodeActivities = new();
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvTrackServer.Models.Database;

public class EpisodeActivity
{
    [Key]
    public int Id { get; set; }
    public int TvMazeId { get; set; }
    
    public bool Watched { get; set; }


    [ForeignKey(nameof(ShowActivity))]
    public int ShowActivityId { get; set; }
    public virtual ShowActivity ShowActivity { get; set; } = null!;
}
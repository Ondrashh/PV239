using System.ComponentModel.DataAnnotations;

namespace TVTrack.Models.Database;

public class UserRatedShow
{
    [Key]
    public int Id { get; set; }
    public int TvMazeId { get; set; }

    public int UserRatingCount { get; set; } = 0;
    public float UserRating { get; set; }
}
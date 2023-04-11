using Newtonsoft.Json;

namespace TvTrackServer.Models.TvMaze;

public class Rating
{
    [JsonProperty("average")]
    public double? Average { get; set; }

    public double? TvTrackRating { get; set; } = null;
    public int TvTrackRatingCount { get; set; } = 0;
}

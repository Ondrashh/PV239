using Newtonsoft.Json;

namespace TvTrackServer.Models.TvMaze;

public class Rating
{
    [JsonProperty("average")]
    public double? Average { get; set; }
}

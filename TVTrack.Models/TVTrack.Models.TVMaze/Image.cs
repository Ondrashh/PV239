using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Image
{
    [JsonProperty("medium")]
    public string Medium { get; set; }

    [JsonProperty("original")]
    public string Original { get; set; }
}

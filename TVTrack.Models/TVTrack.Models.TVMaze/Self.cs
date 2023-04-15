using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Self
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

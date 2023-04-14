using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Link
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

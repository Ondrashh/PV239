using Newtonsoft.Json;

namespace TvTrackServer.Models.TvMaze;

public class Link
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

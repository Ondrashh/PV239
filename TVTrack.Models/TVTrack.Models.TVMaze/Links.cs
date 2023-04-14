using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Links
{
    [JsonProperty("self")]
    public Link Self { get; set; }

    [JsonProperty("previousepisode")]
    public Link PreviousEpisode { get; set; }

    [JsonProperty("show")]
    public Link Show { get; set; }
}

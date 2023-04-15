using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Embedded
{
    [JsonProperty("episodes")]
    public List<Episode>? Episodes { get; set; }

    [JsonProperty("seasons")]
    public List<Season>? Seasons { get; set; }
}

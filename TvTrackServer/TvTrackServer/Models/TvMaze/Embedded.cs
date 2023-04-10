using Newtonsoft.Json;
using TVTrack.TVMaze.Client.Models;

namespace TvTrackServer.Models.TvMaze;

public class Embedded
{
    [JsonProperty("episodes")]
    public List<Episode>? Episodes { get; set; }

    [JsonProperty("seasons")]
    public List<Season>? Seasons { get; set; }
}

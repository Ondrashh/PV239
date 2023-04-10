using Newtonsoft.Json;
using TVTrack.TVMaze.Client.Models;

namespace TvTrackServer.Models.TvMaze;

public class Season
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("number")]
    public int? Number { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("episodeOrder")]
    public int? EpisodeOrder { get; set; }

    [JsonProperty("premiereDate")]
    public string PremiereDate { get; set; }

    [JsonProperty("endDate")]
    public string EndDate { get; set; }

    [JsonProperty("network")]
    public Network? Network { get; set; }

    [JsonProperty("webChannel")]
    public Network? WebChannel { get; set; }

    [JsonProperty("image")]
    public Image Image { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("_links")]
    public Links? Links { get; set; }
}

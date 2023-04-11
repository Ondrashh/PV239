using Newtonsoft.Json;

namespace TvTrackServer.Models.TvMaze;

public class Network
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("country")]
    public Country? Country { get; set; }

    [JsonProperty("officialSite")]
    public string OfficialSite { get; set; }
}

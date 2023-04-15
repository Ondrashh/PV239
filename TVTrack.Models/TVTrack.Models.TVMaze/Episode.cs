using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Episode
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("season")]
    public int Season { get; set; }

    [JsonProperty("number")]
    public int Number { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("airdate")]
    public string Airdate { get; set; }

    [JsonProperty("airtime")]
    public string Airtime { get; set; }

    [JsonProperty("airstamp")]
    public DateTime? Airstamp { get; set; }

    [JsonProperty("runtime")]
    public int? Runtime { get; set; }

    [JsonProperty("rating")]
    public Rating? Rating { get; set; }

    [JsonProperty("image")]
    public Image? Image { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("_links")]
    public Links? Links { get; set; }

    public bool UserWatched { get; set; } = false;
}

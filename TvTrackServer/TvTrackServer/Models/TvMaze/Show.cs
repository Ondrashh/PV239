using Newtonsoft.Json;
using TVTrack.TVMaze.Client.Models;

namespace TvTrackServer.Models.TvMaze;

public class Show
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("language")]
    public string Language { get; set; }

    [JsonProperty("genres")]
    public List<string> Genres { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("runtime")]
    public int? Runtime { get; set; }

    [JsonProperty("averageRuntime")]
    public int? AverageRuntime { get; set; }

    [JsonProperty("premiered")]
    public string Premiered { get; set; }

    [JsonProperty("ended")]
    public string Ended { get; set; }

    [JsonProperty("officialSite")]
    public string? OfficialSite { get; set; }

    [JsonProperty("schedule")]
    public Schedule Schedule { get; set; }

    [JsonProperty("rating")]
    public Rating? Rating { get; set; }

    [JsonProperty("weight")]
    public int Weight { get; set; }

    [JsonProperty("network")]
    public Network? Network { get; set; }

    [JsonProperty("webChannel")]
    public Network? WebChannel { get; set; }

    [JsonProperty("dvdCountry")]
    public Network? DvdCountry { get; set; }

    [JsonProperty("externals")]
    public Externals Externals { get; set; }

    [JsonProperty("image")]
    public Image? Image { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("updated")]
    public int Updated { get; set; }

    [JsonProperty("_links")]
    public Links Links { get; set; }

    [JsonProperty("_embedded")]
    public Embedded? Embedded { get; set; }

    public bool UserRated { get; set; } = false;
    public int? UserRating { get; set; } = null;
    public bool Notifications { get; set; } = false;
    public bool Calendar { get; set; } = false;
}

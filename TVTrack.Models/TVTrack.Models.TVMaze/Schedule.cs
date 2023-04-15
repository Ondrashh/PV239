using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Schedule
{
    [JsonProperty("time")]
    public string Time { get; set; }

    [JsonProperty("days")]
    public List<string> Days { get; set; }
}

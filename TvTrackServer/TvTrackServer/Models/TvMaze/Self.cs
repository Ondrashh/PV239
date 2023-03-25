using Newtonsoft.Json;

namespace TvTrackServer.Models.TvMaze
{
    public class Self
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}

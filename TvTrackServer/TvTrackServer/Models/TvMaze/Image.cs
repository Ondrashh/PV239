using Newtonsoft.Json;

namespace TvTrackServer.Models.TvMaze
{
    public class Image
    {
        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("original")]
        public string Original { get; set; }
    }
}

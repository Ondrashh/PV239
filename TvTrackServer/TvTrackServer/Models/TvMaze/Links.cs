using Newtonsoft.Json;
using TVTrack.TVMaze.Client.Models;

namespace TvTrackServer.Models.TvMaze
{
    public class Links
    {
        [JsonProperty("self")]
        public Link Self { get; set; }

        [JsonProperty("previousepisode")]
        public Link PreviousEpisode { get; set; }

        [JsonProperty("show")]
        public Link Show { get; set; }
    }
}

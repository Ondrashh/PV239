using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.TVMaze.Client.Models
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

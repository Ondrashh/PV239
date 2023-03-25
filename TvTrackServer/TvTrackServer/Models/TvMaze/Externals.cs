using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.TVMaze.Client.Models
{
    public class Externals
    {
        [JsonProperty("tvrage")]
        public int? Tvrage { get; set; }

        [JsonProperty("thetvdb")]
        public int? Thetvdb { get; set; }

        [JsonProperty("imdb")]
        public string? Imdb { get; set; }
    }
}

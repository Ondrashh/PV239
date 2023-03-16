using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.TVMaze.Client.Models
{
    public class Embedded
    {
        [JsonProperty("episodes")]
        public List<Episode>? Episodes { get; set; }

        [JsonProperty("seasons")]
        public List<Season>? Seasons { get; set; }
    }
}

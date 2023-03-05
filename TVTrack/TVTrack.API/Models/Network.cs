using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.API.Models
{
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
}

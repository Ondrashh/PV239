using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.API.Models
{
    public class Image
    {
        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("original")]
        public string Original { get; set; }
    }
}

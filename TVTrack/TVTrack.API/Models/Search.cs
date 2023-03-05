using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.API.Models
{
    public class Search
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("show")]
        public Show Show { get; set; }
    }
}

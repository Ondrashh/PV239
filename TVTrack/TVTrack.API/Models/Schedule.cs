using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.API.Models
{
    public class Schedule
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("days")]
        public List<string> Days { get; set; }
    }
}

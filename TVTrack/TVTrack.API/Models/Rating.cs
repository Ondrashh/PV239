using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.API.Models
{
    public class Rating
    {
        [JsonProperty("average")]
        public double? Average { get; set; }
    }
}

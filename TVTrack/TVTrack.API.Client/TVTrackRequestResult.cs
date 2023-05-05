using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.API.Client
{
    public class TVTrackRequestResult
    {
        public bool Success { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}

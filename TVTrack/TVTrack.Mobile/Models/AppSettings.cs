using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public class AppSettings
    {
        public string FCMServerKey { get; set; }
        public string APIURL { get; set; }
        public string TVMazeURL { get; set; }

        public static AppSettings Get(IConfiguration configuration)
        {
            return configuration.GetRequiredSection("Settings").Get<AppSettings>();
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public class AppSettings
    {
        public string FCMServerKey { get; set; }
        public string APIURL { get; set; }
        public string TVMazeURL { get; set; }
        public GoogleOAuth Google { get; set; } = new();

        public static AppSettings Get(IConfiguration configuration)
        {
            return configuration.GetRequiredSection("Settings").Get<AppSettings>();
        }
    }

    public class GoogleOAuth
    {
        public string ClientID { get; set; }
        public string ProjectID { get; set; }
        public string AuthURI { get; set; }
        public string TokenURI { get; set; }
    }
}

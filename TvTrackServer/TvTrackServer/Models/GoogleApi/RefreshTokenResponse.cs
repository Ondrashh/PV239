using Newtonsoft.Json;

namespace TvTrackServer.Models.GoogleApi
{
    public class RefreshTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}

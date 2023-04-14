using RestSharp;

namespace TVTrack.API.Client
{
    public class TVTrackClient: IDisposable
    {
        private readonly RestClient _client;

        public TVTrackClient(RestClient client)
        {
            _client = client;
        }


        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
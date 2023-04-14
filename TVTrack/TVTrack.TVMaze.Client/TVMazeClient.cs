using Newtonsoft.Json;
using RestSharp;
using TVTrack.Models.TvMaze;

namespace TVTrack.TVMaze.Client
{
    public class TVMazeClient: IDisposable
    {
        private RestClient _client;

        public TVMazeClient(RestClient client)
        {
            _client = client;
        }

        public async Task<ICollection<Search>> Search(string query)
        {
            var request = new RestRequest(TVMazeEndpoints.SEARCH)
                .AddQueryParameter("q", query);
            var response = await _client.GetAsync<ICollection<Search>>(request);

            return response;
        }

        public async Task<Show> GetShowDetails(int id)
        {
            var request = new RestRequest(TVMazeEndpoints.SHOW)
                .AddUrlSegment("id", id)
                .AddQueryParameter("embed[]", "episodes", false)
                .AddQueryParameter("embed[]", "seasons", false);
            var response = await _client.GetAsync(request);

            var show = JsonConvert.DeserializeObject<Show>(response.Content);

            return show;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
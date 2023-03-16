using Newtonsoft.Json;
using RestSharp;
using TVTrack.TVMaze.Client.Models;

namespace TVTrack.TVMaze.Client
{
    public class TVMazeClient
    {
        private RestClient client;

        public TVMazeClient()
        {
            client = new RestClient(Endpoints.URL);
        }

        public async Task<ICollection<Search>> Search(string query)
        {
            var request = new RestRequest(Endpoints.SEARCH)
                .AddQueryParameter("q", query);
            var response = await client.GetAsync<ICollection<Search>>(request);

            return response;
        }

        public async Task<Show> GetShowDetails(int id)
        {
            var request = new RestRequest(Endpoints.SHOW)
                .AddUrlSegment("id", id)
                .AddQueryParameter("embed[]", "episodes", false)
                .AddQueryParameter("embed[]", "seasons", false);
            var response = await client.GetAsync(request);

            return JsonConvert.DeserializeObject<Show>(response.Content);
        }
    }
}
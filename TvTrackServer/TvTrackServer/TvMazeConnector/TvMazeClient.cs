using Newtonsoft.Json;
using RestSharp;
using TVTrack.Models.TvMaze;

namespace TvTrackServer.TvMazeConnector;

public class TvMazeClient
{
    private readonly RestClient _client;

    public TvMazeClient()
    {
        _client = new RestClient(TvMazeEndpoints.URL);
    }

    public async Task<ICollection<Search>> Search(string query)
    {
        var request = new RestRequest(TvMazeEndpoints.SEARCH)
            .AddQueryParameter("q", query);
        var response = await _client.GetAsync<ICollection<Search>>(request);

        return response;
    }

    public async Task<Show> GetShowDetailsWithSeasonsAndEpisodes(int id)
    {
        var request = new RestRequest(TvMazeEndpoints.SHOW)
            .AddUrlSegment("id", id)
            .AddQueryParameter("embed[]", "episodes", false)
            .AddQueryParameter("embed[]", "seasons", false);
        var response = await _client.GetAsync(request);

        var show = JsonConvert.DeserializeObject<Show>(response.Content);

        return show;
    }

    public async Task<Show> GetShowDetails(int id)
    {
        var request = new RestRequest(TvMazeEndpoints.SHOW)
            .AddUrlSegment("id", id);
        var response = await _client.GetAsync(request);

        var show = JsonConvert.DeserializeObject<Show>(response.Content);

        return show;
    }
}

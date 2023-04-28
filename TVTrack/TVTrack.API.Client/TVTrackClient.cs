using Newtonsoft.Json;
using RestSharp;
using TVTrack.Models.Database;
using TVTrack.Models.TvMaze;

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

        public async Task<ICollection<User>> GetUsers()
        {
            var request = new RestRequest(TVTrackEndpoints.USERS);
            request.Method = Method.Get;
            var response = await _client.GetAsync<ICollection<User>>(request);

            return response;
        }

        public async Task<ICollection<Search>> Search(string query)
        {
            var request = new RestRequest(TVTrackEndpoints.SEARCH)
                .AddQueryParameter("search", query);
            request.Method = Method.Get;
            var response = await _client.GetAsync<ICollection<Search>>(request);

            return response;
        }

        public async Task<Show> GetShowDetails(int id)
        {
            var request = new RestRequest(TVTrackEndpoints.SHOW)
                .AddUrlSegment("id", id)
                .AddQueryParameter("embed[]", "episodes", false)
                .AddQueryParameter("embed[]", "seasons", false);
            request.Method = Method.Get;
            var response = await _client.GetAsync<Show>(request);

            return response;
        }

        public async Task ToggleNotifications(int id, string username, bool enabled)
        {
            var request = new RestRequest(TVTrackEndpoints.NOTIFICATION)
                .AddUrlSegment("id", id)
                .AddQueryParameter("username", username)
                .AddBody(new EnabledDto() { Enabled = enabled});
            request.Method = Method.Patch;
            await _client.PatchAsync(request);
        }

        public async Task ToggleCalendar(int id, string username, bool enabled)
        {
            var request = new RestRequest(TVTrackEndpoints.CALENDAR)
                .AddUrlSegment("id", id)
                .AddQueryParameter("username", username)
                .AddBody(new EnabledDto() { Enabled = enabled });
            request.Method = Method.Patch;
            await _client.PatchAsync(request);
        }

        public async Task PostRating(int id, string username, int rating)
        {
            var request = new RestRequest(TVTrackEndpoints.RATE)
                .AddUrlSegment("id", id)
                .AddQueryParameter("username", username)
                .AddQueryParameter("rating", rating);
            request.Method = Method.Post;
            await _client.PostAsync(request);
        }


        public async Task AddWatchNext(int id, string username)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_DEFAULT)
                .AddQueryParameter("tvMazeId", id)
                .AddQueryParameter("username", username);
            request.Method = Method.Post;
            await _client.PostAsync(request);
        }

        public async Task RemoveWatchNext(int id, string username)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_DEFAULT_DELETE)
                .AddUrlSegment("tvMazeId", id)
                .AddQueryParameter("username", username);
            request.Method = Method.Delete;
            await _client.DeleteAsync(request);
        }

        public async Task ToggleWatchedEpisode(int showId, int episodeId, string username, bool watched)
        {
            var request = new RestRequest(TVTrackEndpoints.EPISODE_WATCHED)
                .AddUrlSegment("showId", showId)
                .AddUrlSegment("epId", episodeId)
                .AddQueryParameter("username", username)
                .AddBody(new WatchedDto() { Watched = watched });
            request.Method = Method.Patch;
            await _client.PatchAsync(request);
        }

        public async Task PutGoogleCalendarToken(string username, string accessToken, string refreshToken)
        {
            var request = new RestRequest(TVTrackEndpoints.GCAL_TOKEN)
                .AddUrlSegment("username", username)
                .AddQueryParameter("gcApiToken", accessToken)
                .AddQueryParameter("gcRefreshToken", refreshToken);
            request.Method = Method.Put;
            await _client.PatchAsync(request);
        }

        public async Task PutFCMToken(string username, string deviceToken)
        {
            var request = new RestRequest(TVTrackEndpoints.FCM_TOKEN)
                .AddUrlSegment("username", username)
                .AddQueryParameter("fcmDeviceToken", deviceToken);
            request.Method = Method.Put;
            await _client.PatchAsync(request);
        }
    }
    public class EnabledDto
    {
        public bool Enabled { get; set; }
    }
    public class WatchedDto
    {
        public bool Watched { get; set; }
    }

}
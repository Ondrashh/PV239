using Newtonsoft.Json;
using RestSharp;
using System.Xml.Linq;
using TVTrack.API.Client.Models;
using TVTrack.API.Models;
using TVTrack.Models.Database;
using System.Net;
using TVTrack.Models.API.Responses;
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

        private TVTrackRequestResult CreateTVTrackRequestResult(RestResponse? response)
        {
            if (response == null)
            {
                return new TVTrackRequestResult()
                {
                    Success = false,
                    Reason = "Failed to fetch response from server, was null."
                };
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new TVTrackRequestResult()
                {
                    Success = true
                };
            }

            return new TVTrackRequestResult()
            {
                Success = false,
                Reason = response.Content ?? $"Remote server returned {response.StatusCode}."
            };
        }

        public async Task<ICollection<User>> GetUsers()
        {
            var request = new RestRequest(TVTrackEndpoints.USERS);
            request.Method = Method.Get;
            var response = await _client.GetAsync<ICollection<User>>(request);

            return response;
        }

        public async Task<TVTrackRequestResult> RegisterUser(string username)
        {
            var request = new RestRequest(TVTrackEndpoints.USERS)
                .AddQueryParameter("username", username);
            request.Method = Method.Post;

            var response = await _client.ExecuteAsync(request);

            return CreateTVTrackRequestResult(response);
        }


        public async Task<ICollection<Search>> Search(string query)
        {
            var request = new RestRequest(TVTrackEndpoints.SEARCH)
                .AddQueryParameter("search", query);
            request.Method = Method.Get;
            var response = await _client.GetAsync<ICollection<Search>>(request);

            return response;
        }

        public async Task<Show> GetShowDetails(int id, string username = null)
        {
            var request = new RestRequest(TVTrackEndpoints.SHOW)
                .AddUrlSegment("id", id)
                .AddQueryParameter("embed[]", "episodes", false)
                .AddQueryParameter("embed[]", "seasons", false);
            if (!string.IsNullOrEmpty(username))
            {
                request.AddQueryParameter("username", username);
            }
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
            await _client.PutAsync(request);
        }

        public async Task PutFCMToken(string username, string deviceToken)
        {
            var request = new RestRequest(TVTrackEndpoints.FCM_TOKEN)
                .AddUrlSegment("username", username)
                .AddQueryParameter("fcmDeviceToken", deviceToken);
            request.Method = Method.Put;
            var res = await _client.PutAsync(request);
        }

        public async Task<UserHasTokensModel> GetHasTokens(string username)
        {
            var request = new RestRequest(TVTrackEndpoints.HAS_TOKENS)
                .AddUrlSegment("username", username);
            request.Method = Method.Get;
            var res = await _client.GetAsync<UserHasTokensModel>(request);
            return res;
        }

        public async Task<IEnumerable<ShowListPreview>> GetUserShowsLists(string username)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_USER)
                .AddQueryParameter("username", username);
            request.Method = Method.Get;
            var response = await _client.GetAsync<IEnumerable<ShowListPreview>>(request);

            return response;
        }

        public async Task<ShowListDetail> GetUserShowsDetail(int id, string username)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_DETAIL)
                .AddUrlSegment("id", id)
                .AddQueryParameter("username", username);
            request.Method = Method.Get;
            var response = await _client.GetAsync<ShowListDetail>(request);

            return response;
        }

        public async Task<ShowListDetail> CreateUserShow(string userName, string name, string description)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_CREATE)
                .AddQueryParameter("username", userName)
                .AddBody(new CreateNewDto() { Name = name, Description = description });

            request.Method = Method.Post;
            var response = await _client.PostAsync<ShowListDetail>(request);

            return response;
        }

        public async Task DeleteUserShow(string userName, int id)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_DELETE)
                            .AddQueryParameter("username", userName)
                            .AddUrlSegment("id", id);

            request.Method = Method.Delete;
            await _client.DeleteAsync(request);
        }

        public async Task EditUserShow(string userName, int id, string name, string description)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_UPDATE)
                .AddUrlSegment("id", id)
                .AddQueryParameter("username", userName)
                .AddBody(new CreateNewDto() { Name = name, Description = description });

            request.Method = Method.Put;
            await _client.PutAsync(request);

        }

        public async Task DeleteShowFromUserShow(int listId, int showId)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_DELETE_SHOW)
                 .AddUrlSegment("listId", listId)
                 .AddUrlSegment("showId", showId);

            request.Method = Method.Delete;
            await _client.DeleteAsync(request);
        }

        public async Task<List<ShowListAvailable>> GetAvailalbleUserLists(string username, int id)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_AVAILABLE)
                .AddUrlSegment("username", username)
                .AddUrlSegment("id", id);

            request.Method = Method.Get;
            var response = await _client.GetAsync<List<ShowListAvailable>>(request);

            return response;
        }

        public async Task AddShowToUserShow(int listId, int showId)
        {
            var request = new RestRequest(TVTrackEndpoints.LIST_ADD_NEW_SHOW)
                .AddUrlSegment("listId", listId)
                .AddQueryParameter("tvMazeId", showId);

            request.Method = Method.Post;
            await _client.PostAsync(request);
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
    public class CreateNewDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
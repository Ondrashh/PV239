using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TVTrack.Models.TvMaze;
using TvTrackServer.Models.GoogleApi;
using TvTrackServer.TvMazeConnector;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TvTrackServer.Services
{
    public class TVGoogleCalendarService
    {
        private readonly IConfiguration _configuration;
        private readonly TvTrackServerDbContext _dbContext;
        private readonly TvMazeClient _client;

        public TVGoogleCalendarService(IConfiguration configuration,
            TvTrackServerDbContext dbContext,
            TvMazeClient client)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _client = client;
        }

        public async Task<string> GetNewAccessToken(string refreshToken)
        {
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", "978122374720-5vgmbmtp9kspm92kjhducf09l2a8402p.apps.googleusercontent.com"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            using var client = new HttpClient();
            using var refreshTokenResponse = await client.PostAsync("https://accounts.google.com/o/oauth2/token", parameters);

            if (refreshTokenResponse.IsSuccessStatusCode)
            {
                var data = await refreshTokenResponse.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<RefreshTokenResponse>(data);
                return token.AccessToken;
            }

            throw new Exception("Could not refresh token");
        }

        private CalendarService GetService(string accessToken)
        {
            string[] scopes = {
                CalendarService.Scope.CalendarEvents
            };

            GoogleCredential credential = GoogleCredential.FromAccessToken(accessToken).CreateScoped(scopes);

            var calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "TVTrack.GoogleCalendar",
            });

            return calendarService;
        }


        public async Task Synchronize(string username, string accessToken, string refreshToken, DateTime date,
            Show show)
        {
            var eventId = $"{show.Id}:{date.ToFileTimeUtc()}";

            var newToken = await GetNewAccessToken(refreshToken);

            var calendarService = GetService(newToken);

            var events = await calendarService.Events.List("primary").ExecuteAsync();

            var user = await _dbContext.Users.Include(x => x.Tokens).FirstOrDefaultAsync(x => x.Username == username);
            user.Tokens.GoogleCalendarToken = newToken;
            _dbContext.Attach(user);
            await _dbContext.SaveChangesAsync();

            var exists = events.Items.Any(x => x.Description.Contains(eventId));

            if (exists)
            {
                calendarService.Dispose();
                return;
            }

            var res = await calendarService.Events.Insert(new Event()
            {
                Summary = $"{show.Name} New Episode",
                Description = $"New episode of {show.Name} is premiering today ({eventId})",
                Start = new EventDateTime()
                {
                    DateTime = date,
                    TimeZone = "Europe/Prague"
                },
                End = new EventDateTime()
                {
                    DateTime = date.AddHours(1),
                    TimeZone = "Europe/Prague"
                },
            }, "primary").ExecuteAsync();

            calendarService.Dispose();
        }
    }
}

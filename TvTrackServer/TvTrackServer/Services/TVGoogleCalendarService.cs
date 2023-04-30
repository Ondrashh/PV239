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

        public async Task<string> GetRefreshToken(string refreshToken)
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

        public async Task Debug()
        {
            var show = await _client.GetShowDetails(23470);
            var date = DateTime.Today;

            var eventId = $"285:{date.ToFileTimeUtc()}";

            CalendarService calendarService;
            Events events;

            try
            {
                calendarService = GetService("ya29.a0AWY7Ckl9bnsbh5s7rs23TrPwxG_nv7Xu0XpWs9fNg6VnSn0UxpS733x2JEFvqGJT8G8qC1R1PQx-hnKJXlSCJ9XUqTM1wkBvfH7r4eIHuWmLKhS-boJdmFAERShehlra4EJ868RfKGX4W1ZhVC67SgDP_tX0aCgYKAZ0SARASFQG1tDrps9hfsBgW8mq1Ryf1NIuKUQ0163");

                events = await calendarService.Events.List("primary").ExecuteAsync();
            }
            catch (Google.GoogleApiException)
            {
                var newToken = await GetRefreshToken("1//09ySjd8jNSE64CgYIARAAGAkSNwF-L9IrQ6lH4Sz4df9JWvqOC9WdEwiXOAE-pxIMozI3aRYKs90mOM25ZOQJu0_zVWvOCT61nIQ");

                calendarService = GetService(newToken);

                events = await calendarService.Events.List("primary").ExecuteAsync();
            }

            var exists = events.Items.Any(x => x.Description.Contains(eventId));

            if (exists)
            {
                calendarService.Dispose();
                return;
            }

            var res = await calendarService.Events.Insert(new Event()
            {
                Summary = "Debug Event",
                Description = $"A debug event created from TV Track Web Service ({eventId})",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Now,
                    TimeZone = "Europe/Prague"
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Now.AddHours(1),
                    TimeZone = "Europe/Prague"
                },
            }, "primary").ExecuteAsync();

            calendarService.Dispose();
        }

        public async Task Synchronize(string username, string accessToken, string refreshToken, DateTime date, Show show)
        {
            var eventId = $"{show.Id}:{date.ToFileTimeUtc()}";

            CalendarService calendarService;
            Events events;

            try
            {
                calendarService = GetService(accessToken);

                events = await calendarService.Events.List("primary").ExecuteAsync();
            }
            catch (Google.GoogleApiException)
            {
                var newToken = await GetRefreshToken(refreshToken);

                calendarService = GetService(newToken);

                events = await calendarService.Events.List("primary").ExecuteAsync();

                var user = await _dbContext.Users.Include(x => x.Tokens).FirstOrDefaultAsync(x => x.Username == username);
                user.Tokens.GoogleCalendarToken = newToken;
                _dbContext.Attach(user);
                await _dbContext.SaveChangesAsync();
            }

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

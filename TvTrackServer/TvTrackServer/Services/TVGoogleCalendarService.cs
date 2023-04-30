using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace TvTrackServer.Services
{
    public class TVGoogleCalendarService
    {
        private readonly IConfiguration _configuration;

        public TVGoogleCalendarService(IConfiguration configuration)
        {
            _configuration = configuration;
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
            var calendarService = GetService("");

            calendarService.Calendars.Get("TVTrack");

            return;
        }

        public async Task Synchronize(string accessToken, string refreshToken, DateTime date, string name)
        {
            var calendarService = GetService("");

            calendarService.Calendars.Get("TVTrack");

            return;
        }
    }
}

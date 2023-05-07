using Microsoft.EntityFrameworkCore;
using Quartz;
using TvTrackServer.Helpers;
using TvTrackServer.Services;
using TvTrackServer.TvMazeConnector;

namespace TvTrackServer.Jobs
{
    public class GoogleCalendarSyncJob : IJob
    {
        private readonly TvTrackServerDbContext _dbContext;
        private readonly TVGoogleCalendarService _calendarService;
        private readonly TvMazeClient _tvMazeClient;

        public GoogleCalendarSyncJob(TvTrackServerDbContext dbContext,
            TVGoogleCalendarService calendarService,
            TvMazeClient tvMazeClient)
        {
            _dbContext = dbContext;
            _calendarService = calendarService;
            _tvMazeClient = tvMazeClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var syncableActivities = await _dbContext.ShowActivities
                .Include(x => x.User)
                .ThenInclude(x => x.Tokens)
                .Where(x => x.Notifications
                            && x.User.Tokens.GoogleCalendarToken != null && x.User.Tokens.GoogleCalendarRefreshToken != null
                            && x.NextNotifyDate >= DateTime.Today && x.NextNotifyDate < DateTime.Today.AddDays(1))
                .ToListAsync();

            foreach (var activity in syncableActivities)
            {
                // get show from api
                var show = await _tvMazeClient.GetShowDetails(activity.TvMazeId);

                await _calendarService.Synchronize(activity.User.Username,
                    activity.User.Tokens.GoogleCalendarToken,
                    activity.User.Tokens.GoogleCalendarRefreshToken,
                    DateTime.Today,
                    show);

                var nextDate = show.GetNextEpisodeDate();
                if (!string.IsNullOrEmpty(show.Schedule?.Days.FirstOrDefault())
                    && nextDate > activity.NextNotifyDate)
                {
                    activity.NextNotifyDate = nextDate;
                }

                _dbContext.Attach(activity);

                await Task.Delay(500);
            }

            await _dbContext.SaveChangesAsync();

            Console.WriteLine($"[GoogleCalendarJob]: {DateTime.Now.ToLongTimeString()}");
        }
    }
}

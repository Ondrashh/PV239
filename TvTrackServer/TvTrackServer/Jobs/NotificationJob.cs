using Microsoft.EntityFrameworkCore;
using Quartz;
using TvTrackServer.Helpers;
using TvTrackServer.Services;
using TvTrackServer.TvMazeConnector;

namespace TvTrackServer.Jobs
{
    public class NotificationJob : IJob
    {
        private readonly TvTrackServerDbContext _dbContext;
        private readonly NotificationService _notificationService;
        private readonly TvMazeClient _tvMazeClient;

        public NotificationJob(TvTrackServerDbContext dbContext,
            NotificationService notificationService,
            TvMazeClient tvMazeClient)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
            _tvMazeClient = tvMazeClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // TODO test notification job

            var notifiableActivities = await _dbContext.ShowActivities
                .Include(x => x.User)
                .ThenInclude(x => x.Tokens)
                .Where(x => x.Notifications
                        && x.User.Tokens.FCMDeviceToken != null
                        && x.NextNotifyDate >= DateTime.Today && x.NextNotifyDate < DateTime.Today.AddDays(1))
                .ToListAsync();


            foreach (var activity in notifiableActivities)
            {
                var show = await _tvMazeClient.GetShowDetails(activity.TvMazeId);

                await _notificationService.Notify(activity.User.Tokens.FCMDeviceToken,
                    "New episode alert!",
                    $"New episode of {show.Name} is premiering today!");



                var nextDate = show.GetLatestEpisodeDate();
                if (!string.IsNullOrEmpty(show.Schedule?.Days.FirstOrDefault())
                    && nextDate > activity.NextNotifyDate)
                {
                    activity.NextNotifyDate = nextDate;
                }

                await Task.Delay(500);
            }

            _dbContext.Attach(notifiableActivities);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine(DateTime.Now.ToLongTimeString());
        }
    }
}

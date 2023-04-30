using TVTrack.Models.TvMaze;

namespace TvTrackServer.Helpers
{
    public static class ShowHelper
    {
        public static DateTime GetLatestEpisodeDateTime(this Show show)
        {
            var date = DateTimeHelper.GetByDayOfWeek(show.Schedule.Days.FirstOrDefault() ?? "Monday");

            if (show.Status == "Ended" && !string.IsNullOrEmpty(show.Ended))
            {
                return DateTime.Parse(show.Ended);
            }

            return date;
        }
    }
}

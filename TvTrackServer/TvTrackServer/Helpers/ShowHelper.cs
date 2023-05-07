using TVTrack.Models.TvMaze;

namespace TvTrackServer.Helpers
{
    public static class ShowHelper
    {
        public static DateTime GetLatestEpisodeDate(this Show show)
        {
            var date = DateTimeHelper.GetByDayOfWeek(show.Schedule.Days.FirstOrDefault() ?? "Monday");

            if (show.Status == "Ended" && !string.IsNullOrEmpty(show.Ended))
            {
                return DateTime.Parse(show.Ended);
            }

            return date;
        }

        public static DateTime GetNextEpisodeDate(this Show show)
        {
            var date = DateTimeHelper.GetByDayOfWeek(show.Schedule.Days.FirstOrDefault() ?? "Monday");

            if (show.Status == "Ended" && !string.IsNullOrEmpty(show.Ended))
            {
                return DateTime.Parse(show.Ended);
            }

            if (date == DateTime.Today)
            {
                date = date.AddDays(7);
            }

            return date;
        }
    }
}

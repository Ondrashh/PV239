namespace TvTrackServer.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime GetByDayOfWeek(string dayOfWeek)
        {
            var parsed = Enum.TryParse(dayOfWeek, true, out DayOfWeek dow);
            if (!parsed)
            {
                throw new Exception($"Could not parse day of week {dayOfWeek}");
            }

            var todayDow = DateTime.Today.DayOfWeek;

            var offset = dow - todayDow;
            if (offset < 0)
            {
                offset = (int)DayOfWeek.Saturday + offset + 1;
            }

            return DateTime.Today.AddDays(offset);
        }
    }
}

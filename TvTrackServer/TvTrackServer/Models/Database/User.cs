namespace TvTrackServer.Models.Database
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }

        public List<ShowList> ShowLists = new();
        public List<ShowActivity> ShowActivities = new();
    }
}

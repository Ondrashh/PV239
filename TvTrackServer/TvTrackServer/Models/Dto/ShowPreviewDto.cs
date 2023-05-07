
using TVTrack.Models.TvMaze;

namespace TvTrackServer.Models.Dto
{
    public class ShowPreviewDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public List<string> Genres { get; set; }
        public Rating? Rating { get; set; }
        public Image? Image { get; set; }
        public bool? UserRated { get; set; } = null;
        public int? UserRating { get; set; } = null;
        public bool? Notifications { get; set; } = null;
        public bool? Calendar { get; set; } = null;
        public bool? InUsersDefaultList { get; set; } = null;
    }
}

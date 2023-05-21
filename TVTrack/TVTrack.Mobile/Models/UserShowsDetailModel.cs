using CommunityToolkit.Mvvm.ComponentModel;
using TVTrack.Models.Database;
using TVTrack.Models.TvMaze;

namespace TVTrack.Mobile.Models
{
    public partial class UserShowsDetailModel: ObservableObject
    {
        [ObservableProperty]
        public int id;

        [ObservableProperty]
        public string? name;

        [ObservableProperty]
        public string? description;

        [ObservableProperty]
        public bool isDefault;

        [ObservableProperty]
        public int userId;

        [ObservableProperty]
        public User? user;

        [ObservableProperty]
        public List<Show> shows;
    }
}

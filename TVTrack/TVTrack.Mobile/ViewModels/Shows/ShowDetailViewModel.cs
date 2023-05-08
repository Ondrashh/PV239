using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using TVTrack.Mobile.Models;
using CommunityToolkit.Mvvm.Input;
using TVTrack.Mobile.Views.Popup;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;
using TVTrack.Models.TvMaze;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(Id), "id")]
    public partial class ShowDetailViewModel: ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVTrackClient _client;
        private readonly PopupHelper _popupHelper;
        private string _username = "";

        [ObservableProperty]
        public ShowDetailModel show;

        [ObservableProperty] 
        public bool hasManagedShow;

        [ObservableProperty]
        public bool hasShowEnded;

        [ObservableProperty]
        public bool isShowRunning;

        [ObservableProperty] 
        public bool isShowWatched;

        public ShowDetailViewModel(TVTrackClient client,
            PopupHelper popupHelper,
            IMapper mapper) : base(mapper)
        {
            _client = client;
            _popupHelper = popupHelper;
        }

        public override async Task OnAppearingAsync()
        {
            LoadingStart();
            _username = await StorageHelper.GetUsername();
            var apiShow = await _client.GetShowDetails(Id, _username);
            HasManagedShow = (apiShow.UserRated ?? false)
                || (apiShow.InUsersDefaultList ?? false)
                || (apiShow.Notifications ?? false)
                || (apiShow.Calendar ?? false);
            HasShowEnded = apiShow.Status == "Ended";
            IsShowRunning = apiShow.Status == "Running";
            IsShowWatched = apiShow.Embedded.Episodes.All(x => x.UserWatched);
            Show = _mapper.Map<ShowDetailModel>(apiShow);
            LoadingEnd();
        }

        [RelayCommand]
        public async Task OpenSeasonAsync(int number)
        {
            LoadingStart();
            SeasonModel season = Show.Seasons.FirstOrDefault(x => x.Number == number);
            List<EpisodeModel> episodes = Show.Episodes.Where(x => x.Season == number).ToList();

            await Shell.Current.GoToAsync("season", new Dictionary<string, object>
            {
                ["showId"] = show.Id,
                ["season"] = season,
                ["episodes"] = episodes,
            });
            LoadingEnd();
        }


        [RelayCommand]
        public async Task AddToUserList()
        {
            await Shell.Current.GoToAsync("add", new Dictionary<string, object>
            {
                ["Id"] = Id,
            });
        }

        [RelayCommand]
        public async Task AddToListAsync()
        {
            await _popupHelper.ShowPopupAsync<AddShowPopup>(Id);
        }

        [RelayCommand]
        public async Task MarkShowAsWatchedAsync()
        {
            LoadingStart();
            await _client.MarkShowAsWatched(Show.id, _username, !isShowWatched);

            IsShowWatched = !isShowWatched;

            foreach (var ep in Show.Episodes)
            {
                ep.Watched = IsShowWatched;
            }

            foreach (var season in Show.Seasons)
            {
                season.WatchedEpisodes = IsShowWatched ? season.EpisodeOrder : 0;
            }
            LoadingEnd();
        }
    }
}

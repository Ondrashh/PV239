﻿using AutoMapper;
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

        [ObservableProperty]
        public ShowDetailModel show;

        [ObservableProperty] 
        public bool hasManagedShow;

        [ObservableProperty]
        public bool hasShowEnded;

        [ObservableProperty]
        public bool isShowRunning;

        public ShowDetailViewModel(TVTrackClient client,
            PopupHelper popupHelper,
            IMapper mapper) : base(mapper)
        {
            _client = client;
            _popupHelper = popupHelper;
        }

        public override async Task OnAppearingAsync()
        {
            var username = await StorageHelper.GetUsername();
            var apiShow = await _client.GetShowDetails(Id, username);
            HasManagedShow = (apiShow.UserRated ?? false)
                || (apiShow.InUsersDefaultList ?? false)
                || (apiShow.Notifications ?? false)
                || (apiShow.Calendar ?? false);
            HasShowEnded = apiShow.Status == "Ended";
            IsShowRunning = apiShow.Status == "Running";
            Show = _mapper.Map<ShowDetailModel>(apiShow);
        }

        [RelayCommand]
        public async Task OpenSeasonAsync(int number)
        {
            SeasonModel season = Show.Seasons.FirstOrDefault(x => x.Number == number);
            List<EpisodeModel> episodes = Show.Episodes.Where(x => x.Season == number).ToList();

            await Shell.Current.GoToAsync("season", new Dictionary<string, object>
            {
                ["showId"] = show.Id,
                ["season"] = season,
                ["episodes"] = episodes,
            });
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
    }
}

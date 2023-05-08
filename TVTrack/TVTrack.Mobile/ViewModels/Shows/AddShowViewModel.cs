using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;
using TVTrack.Models.API.Responses;
using TVTrack.Models.TvMaze;
using CommunityToolkit.Maui.Views;

namespace TVTrack.Mobile.ViewModels.Shows
{
    public partial class AddShowViewModel : ViewModelBase
    {
        private readonly TVTrackClient _client;
        private string _username = "";

        public AddShowViewModel(TVTrackClient client, 
            IMapper mapper) : base(mapper)
        {
            _client = client;
        }

        private UserHasTokensModel userHasTokens;

        [ObservableProperty] 
        public Show showDetails;

        [ObservableProperty]
        public int rating;

        [ObservableProperty]
        public bool isNotificationEnabled;

        [ObservableProperty] 
        public bool isAddedToList;

        [ObservableProperty] 
        public bool isAddedToCalendar;

        public override async Task OnAppearingAsync()
        {
            _username = await StorageHelper.GetUsername();

            ShowDetails = await _client.GetShowDetails(ItemID, _username);
            userHasTokens = await _client.GetHasTokens(_username);

            Rating = ShowDetails.UserRating ?? 0;
            IsNotificationEnabled = ShowDetails.Notifications ?? false;
            IsAddedToList = ShowDetails.InUsersDefaultList ?? false;
            IsAddedToCalendar = ShowDetails.Calendar ?? false;
        }

        [RelayCommand]
        public async Task ToggleNotificationsAsync()
        {
            IsNotificationEnabled = !isNotificationEnabled;
            await _client.ToggleNotifications(ItemID, _username, IsNotificationEnabled);
        }

        [RelayCommand]
        public async Task ToggleCalendarAsync()
        {
            if (!userHasTokens.HasGoogleCalendar)
            {
                await AlertHelper.ShowToast("Please log in to Google Calendar before enabling synchronization");
                return;
            }

            IsAddedToCalendar = !isAddedToCalendar;
            await _client.ToggleCalendar(ItemID, _username, IsAddedToCalendar);
        }

        [RelayCommand]
        public async Task ToggleDefaultListAsync()
        {
            if (IsAddedToList)
            {
                await _client.RemoveWatchNext(ItemID, _username);
                IsAddedToList = false;
            }
            else
            {
                await _client.AddWatchNext(ItemID, _username);
                IsAddedToList = true;
            }
        }

        [RelayCommand]
        public async Task SaveShowDetailsAsync()
        {
            await _client.PostRating(ItemID, _username, Rating);
        }

        [RelayCommand]
        public void IncrementRating()
        {
            if (Rating >= 5)
            {
                Rating = 5;
                return;
            }

            Rating += 1;
        }

        [RelayCommand]
        public void DecrementRating()
        {
            if (Rating <= 0)
            {
                Rating = 0;
                return;
            }

            Rating -= 1;
        }

        [RelayCommand]
        public async Task AddToUserList()
        {
            PopupHelper.CloseLastPopup();
            await Shell.Current.GoToAsync("add", new Dictionary<string, object>
            {
                ["Id"] = ItemID,
            });
        }
    }
}

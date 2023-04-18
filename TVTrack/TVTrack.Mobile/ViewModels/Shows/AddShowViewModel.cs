using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TVTrack.API.Client;
using TVTrack.Models.TvMaze;

namespace TVTrack.Mobile.ViewModels.Shows
{
    public partial class AddShowViewModel : ViewModelBase
    {
        private readonly TVTrackClient _client;
        private string _username = "TODO";

        public AddShowViewModel(TVTrackClient client, 
            IMapper mapper) : base(mapper)
        {
            _client = client;
        }

        [ObservableProperty] 
        public Show showDetails;

        [ObservableProperty]
        public int rating;

        [ObservableProperty]
        public bool enableNotifications;

        [ObservableProperty] 
        public bool isAddedToList;

        [ObservableProperty] 
        public bool addToCalendar;

        public override async Task OnAppearingAsync()
        {
            ShowDetails = await _client.GetShowDetails(ItemID);
            EnableNotifications = ShowDetails.Notifications ?? false;
            IsAddedToList = ShowDetails.InUsersDefaultList ?? false;
            AddToCalendar = ShowDetails.Calendar ?? false;

            // TODO USER MANAGEMENT!!!!!
            _username = "TODO!";
        }

        [RelayCommand]
        public async Task ToggleNotificationsAsync()
        {
            await _client.ToggleNotifications(ItemID, _username, EnableNotifications);
        }

        [RelayCommand]
        public async Task ToggleCalendarAsync()
        {
            await _client.ToggleCalendar(ItemID, _username, AddToCalendar);
        }

        [RelayCommand]
        public async Task ToggleDefaultListAsync()
        {
            if (IsAddedToList)
            {
                await _client.AddWatchNext(ItemID, _username);
            }
            else
            {
                await _client.RemoveWatchNext(ItemID, _username);
            }
        }

        [RelayCommand]
        public async Task SaveShowDetailsAsync()
        {
            await _client.PostRating(ItemID, _username, Rating);
        }
    }
}

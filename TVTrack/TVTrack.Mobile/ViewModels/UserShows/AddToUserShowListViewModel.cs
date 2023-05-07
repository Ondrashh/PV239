using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TVTrack.API.Client;
using TVTrack.API.Client.Models;
using TVTrack.Mobile.Helpers;

namespace TVTrack.Mobile.ViewModels.UserShows
{
    [QueryProperty(nameof(Id), "Id")]
    public partial class AddToUserShowListViewModel : ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVTrackClient _client;
        

        [ObservableProperty]
        public List<ShowListAvailable> titles;

        public AddToUserShowListViewModel(TVTrackClient client, IMapper mapper) : base(mapper)
        {
            _client = client;

        }

        public override async Task OnAppearingAsync()
        {
            var username = await StorageHelper.GetUsername();
            var availableuserLists = await _client.GetAvailalbleUserLists(username, Id);
            Titles = availableuserLists;
            await base.OnAppearingAsync();
        }

        [RelayCommand]
        public async Task AssignButtonClicked(ShowListAvailable selectedShowList)
        {
            if (selectedShowList == null)
            {
                await AlertHelper.ShowErrorSnackbar("You have to user list.");
                return;
            }
            await _client.AddShowToUserShow(selectedShowList.Id, Id);

            await Shell.Current.GoToAsync("///userLists", new Dictionary<string, object>
            {
            });
        }

    }
}

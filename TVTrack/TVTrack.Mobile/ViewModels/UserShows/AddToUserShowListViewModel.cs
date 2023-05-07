using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TVTrack.API.Client;
using TVTrack.API.Client.Models;

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

            var availableuserLists = await _client.GetAvailalbleUserLists("test", Id);
            Titles = availableuserLists;
            await base.OnAppearingAsync();
        }

        [RelayCommand]
        public async Task AssignButtonClicked(ShowListAvailable selectedShowList)
        {
            if (selectedShowList == null)
            {
                return;
            }
            await _client.AddShowToUserShow(selectedShowList.Id, Id);

            await Shell.Current.GoToAsync("///userLists", new Dictionary<string, object>
            {
            });
        }

    }
}

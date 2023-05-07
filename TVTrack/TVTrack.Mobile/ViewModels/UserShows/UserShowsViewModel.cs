using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.UserShows
{
    public partial class UserShowsViewModel : ViewModelBase
    {

        private readonly TVTrackClient _client;

        public ObservableCollection<ShowListPreviewModel> Results { get; set; } = new ObservableCollection<ShowListPreviewModel>();

        public UserShowsViewModel(TVTrackClient client, IMapper mapper) : base(mapper)
        {
            _client = client;

        }

        public override async Task OnAppearingAsync()
        {
            var username = await StorageHelper.GetUsername();
            var results = await _client.GetUserShowsLists(username);

            var resMapped = _mapper.Map<IEnumerable<ShowListPreviewModel>>(results);
            Results.Clear();
            foreach (var searchResult in resMapped)
            {
                if (!Results.Contains(searchResult))
                {
                    Results.Add(searchResult);
                }
            }
        }

        [RelayCommand]
        public async Task OpenShowDetailAsync(int id)
        {

            await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
            {
                [nameof(id)] = id
            });
        }

        [RelayCommand]
        public async Task AddNew()
        {
            await Shell.Current.GoToAsync("new", new Dictionary<string, object>
            {
            });
        }
    }
}

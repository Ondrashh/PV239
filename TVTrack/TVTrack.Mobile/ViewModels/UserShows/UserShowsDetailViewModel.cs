using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TVTrack.API.Client;
using TVTrack.API.Client.Models;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.UserShows
{
    [QueryProperty(nameof(Id), "id")]
    public partial class UserShowsDetailViewModel : ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVTrackClient _client;

        [ObservableProperty]
        public UserShowsDetailModel userShowsDetail;

        public UserShowsDetailViewModel(TVTrackClient client, IMapper mapper) : base(mapper)
        {
            _client = client;
        }

        public override async Task OnAppearingAsync()
        {
            var username = await StorageHelper.GetUsername();

            var results = await _client.GetUserShowsDetail(Id, username);

            UserShowsDetail = _mapper.Map<UserShowsDetailModel> (results);
        }


        [RelayCommand]
        public async Task OpenShowDetailAsync(int id)
        {
            await Shell.Current.GoToAsync("show", new Dictionary<string, object>
            {
                [nameof(id)] = id
            });
        }

        [RelayCommand]
        public async Task Edit()
        {
            var Id = userShowsDetail.Id;
            await Shell.Current.GoToAsync("edit", new Dictionary<string, object>
            {
                [nameof(Id)] = Id
            });
        }

        [RelayCommand]
        public async Task Delete()
        {
            var username = await StorageHelper.GetUsername();
            await _client.DeleteUserShow(username, UserShowsDetail.Id);
            await Shell.Current.GoToAsync("///userLists", new Dictionary<string, object>
            {
            });
        }

        [RelayCommand]
        public async Task RemoveShow(int id)
        {
            var username = await StorageHelper.GetUsername();

            await _client.DeleteShowFromUserShow(UserShowsDetail.Id, id);
            var results = await _client.GetUserShowsDetail(Id, username);

            UserShowsDetail = _mapper.Map<UserShowsDetailModel>(results);
        }
    }
}

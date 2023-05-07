using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.UserShows
{
    [QueryProperty(nameof(Id), "Id")]
    public partial class EditUserShowViewModel : ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVTrackClient _client;
        
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public UserShowsDetailModel userShowsDetail;

        public EditUserShowViewModel(TVTrackClient client, IMapper mapper) : base(mapper)
        {
            _client = client;

        }

        public override async Task OnAppearingAsync()
        {
            var username = await StorageHelper.GetUsername();

            var results = await _client.GetUserShowsDetail(Id, username);

            UserShowsDetail = _mapper.Map<UserShowsDetailModel>(results);
        }


    }
}

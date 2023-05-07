using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TVTrack.API.Client;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.UserShows
{
    public partial class AddNewUserShowViewModel : ViewModelBase
    {

        private readonly TVTrackClient _client;
        
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string description;


        public AddNewUserShowViewModel(TVTrackClient client, IMapper mapper) : base(mapper)
        {
            _client = client;

        }

        public override async Task OnAppearingAsync()
        {
            //var results = await _client.GetUserShowsLists("test");

            //var resMapped = _mapper.Map<IEnumerable<ShowListPreviewModel>>(results);
            //Results.Clear();
            //foreach (var searchResult in resMapped)
            //{
            //    if (!Results.Contains(searchResult))
            //    {
            //        Results.Add(searchResult);
            //    }
            //}
        }

    }
}

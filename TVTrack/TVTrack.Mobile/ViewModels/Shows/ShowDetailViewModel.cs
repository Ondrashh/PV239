using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.TVMaze.Client;
using TVTrack.Mobile.Models;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TVTrack.Mobile.Views.Popup;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.DependencyInjection;
using TVTrack.Mobile.Helpers;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(Id), "id")]
    public partial class ShowDetailViewModel: ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVMazeClient _client;
        private readonly PopupHelper _popupHelper;

        [ObservableProperty]
        public ShowDetailModel show;

        public ShowDetailViewModel(TVMazeClient client,
            PopupHelper popupHelper,
            IMapper mapper) : base(mapper)
        {
            _client = client;
            _popupHelper = popupHelper;
        }

        public override async Task OnAppearingAsync()
        {
            var apiShow = await _client.GetShowDetails(Id);
            Show = _mapper.Map<ShowDetailModel>(apiShow);
        }

        [RelayCommand]
        public async Task OpenSeasonAsync(int number)
        {
            SeasonModel season = Show.Seasons.FirstOrDefault(x => x.Number == number);
            List<EpisodeModel> episodes = Show.Episodes.Where(x => x.Season == number).ToList();

            await Shell.Current.GoToAsync("season", new Dictionary<string, object>
            {
                ["season"] = season,
                ["episodes"] = episodes,
            });
        }


        [RelayCommand]
        public async Task AddToListAsync()
        {
            await _popupHelper.ShowPopupAsync<AddShowPopup>(Id);
        }
    }
}

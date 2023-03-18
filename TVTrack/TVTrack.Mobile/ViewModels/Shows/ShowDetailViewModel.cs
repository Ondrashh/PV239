using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.TVMaze.Client;
using TVTrack.TVMaze.Client.Models;
using TVTrack.Mobile.Models;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(Id), "id")]
    public partial class ShowDetailViewModel: ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVMazeClient _client;

        [ObservableProperty]
        public ShowDetailModel show;

        public ShowDetailViewModel(TVMazeClient client,
            IMapper mapper) : base(mapper)
        {
            _client = client;
        }

        public override async Task OnAppearingAsync()
        {
            var apiShow = await _client.GetShowDetails(Id);
            Show = _mapper.Map<ShowDetailModel>(apiShow);
        }

        [RelayCommand]
        public async Task OpenSeasonAsync(int number)
        {
            SeasonModel season = Show.Seasons.FirstOrDefault();
            List<EpisodeModel> episodes = Show.Episodes.Where(x => x.Season == number).ToList();

            await Shell.Current.GoToAsync("season", new Dictionary<string, object>
            {
                ["season"] = season,
                ["episodes"] = episodes,
            });
        }
    }
}

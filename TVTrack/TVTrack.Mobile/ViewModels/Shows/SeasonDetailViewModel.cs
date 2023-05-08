using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(ShowId), "showId")]
    [QueryProperty(nameof(Season), "season")]
    [QueryProperty(nameof(Episodes), "episodes")]
    public partial class SeasonDetailViewModel : ViewModelBase
    {
        private readonly TVTrackClient _client;
        private string _username = "";

        public SeasonDetailViewModel(TVTrackClient client, 
            IMapper mapper) : base(mapper)
        {
            _client = client;
        }

        [ObservableProperty]
        public int showId;
        [ObservableProperty]
        public SeasonModel season;
        [ObservableProperty]
        public List<EpisodeModel> episodes;

        [ObservableProperty] 
        public bool isSeasonWatched;

        public override async Task OnAppearingAsync()
        {
            _username = await StorageHelper.GetUsername();

            IsSeasonWatched = episodes.All(x => x.Watched);
        }

        [RelayCommand]
        public async Task MarkEpisodeAsWatchedAsync(int id)
        {
            var episode = episodes.FirstOrDefault(x => x.id == id);
            episode.Watched = !episode.watched;
            await _client.ToggleWatchedEpisode(showId, id, _username, episode.Watched);
        }

        [RelayCommand]
        public async Task MarkSeasonAsWatchedAsync()
        {
            if (season.number == null)
            {
                return;
            }

            await _client.MarkSeasonAsWatched(showId, season.number ?? 1, _username, !isSeasonWatched);

            IsSeasonWatched = !isSeasonWatched;

            foreach (var ep in Episodes)
            {
                ep.Watched = IsSeasonWatched;
            }
        }
    }
}

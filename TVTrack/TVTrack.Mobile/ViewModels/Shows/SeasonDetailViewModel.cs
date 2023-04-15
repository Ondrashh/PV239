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
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(ShowId), "showId")]
    [QueryProperty(nameof(Season), "season")]
    [QueryProperty(nameof(Episodes), "episodes")]
    public partial class SeasonDetailViewModel : ViewModelBase
    {
        private readonly TVTrackClient _client;
        private string _username = "TODO";

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

        public override Task OnAppearingAsync()
        {
            // TODO ADD USER MANAGEMENT
            _username = "TODO!";
            return base.OnAppearingAsync();
        }

        [RelayCommand]
        public async Task OpenEpisodeDetailAsync(Image chevron)
        {
        }

        [RelayCommand]
        public async Task MarkEpisodeAsWatchedAsync(int id)
        {
            var episode = episodes.FirstOrDefault(x => x.id == id);
            episode.Watched = !episode.watched;
            await _client.ToggleWatchedEpisode(showId, id, _username, episode.Watched);
        }
    }
}

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(SeasonJson), "season")]
    [QueryProperty(nameof(EpisodesJson),"episodes")]
    public partial class SeasonDetailViewModel : ViewModelBase
    {
        public SeasonDetailViewModel(IMapper mapper) : base(mapper)
        {

        }

        public string SeasonJson { get; set; }
        public string EpisodesJson { get; set; }

        [ObservableProperty]
        public SeasonModel season;
        [ObservableProperty]
        public ObservableCollection<EpisodeModel> episodes;

        public override Task OnAppearingAsync()
        {
            Season = JsonConvert.DeserializeObject<SeasonModel>(SeasonJson);
            Episodes = JsonConvert.DeserializeObject<ObservableCollection<EpisodeModel>>(EpisodesJson);

            return base.OnAppearingAsync();
        }
    }
}

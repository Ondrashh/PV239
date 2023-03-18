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
    [QueryProperty(nameof(Season), "season")]
    [QueryProperty(nameof(Episodes), "episodes")]
    public partial class SeasonDetailViewModel : ViewModelBase
    {
        public SeasonDetailViewModel(IMapper mapper) : base(mapper)
        {
        }

        public string EpisodesJson { get; set; }

        [ObservableProperty]
        public SeasonModel season;
        [ObservableProperty]
        public List<EpisodeModel> episodes;

        public override Task OnAppearingAsync()
        {
            return base.OnAppearingAsync();
        }
    }
}

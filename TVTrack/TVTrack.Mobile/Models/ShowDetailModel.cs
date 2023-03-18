using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public partial class ShowDetailModel: ObservableObject
    {
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string summary;

        [ObservableProperty]
        public string imageURL;

        [ObservableProperty]
        public ObservableCollection<SeasonModel> seasons;

        [ObservableProperty]
        public ObservableCollection<EpisodeModel> episodes;
    }
}

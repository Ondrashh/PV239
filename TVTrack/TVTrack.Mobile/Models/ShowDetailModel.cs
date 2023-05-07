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
        public int id;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string summary;

        [ObservableProperty]
        public string imageURL;

        [ObservableProperty]
        public DateTime? premiered;

        [ObservableProperty]
        public DateTime? ended;

        [ObservableProperty]
        public string status;

        [ObservableProperty]
        public string language;

        [ObservableProperty]
        public int? averageRuntime;

        [ObservableProperty]
        public string type;

        [ObservableProperty]
        public string officialSite;

        [ObservableProperty]
        public string network;

        [ObservableProperty]
        public double? averageRating;

        [ObservableProperty]
        public string schedule;

        [ObservableProperty]
        public ObservableCollection<string> genres;

        [ObservableProperty]
        public ObservableCollection<SeasonModel> seasons;

        [ObservableProperty]
        public ObservableCollection<EpisodeModel> episodes;

    }
}

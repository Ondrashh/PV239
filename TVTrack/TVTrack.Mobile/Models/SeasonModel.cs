using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public partial class SeasonModel: ObservableObject
    {
        [ObservableProperty]
        public int id;

        [ObservableProperty]
        public int? number;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string imageUrl;

        [ObservableProperty]
        public string summary;

        [ObservableProperty]
        public int? episodeOrder;

        [ObservableProperty]
        public DateTime? premiered;

        [ObservableProperty]
        public DateTime? ended;

        [ObservableProperty]
        public string network;

        [ObservableProperty] 
        public int? watchedEpisodes;

        public string FormattedName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return $"Season {Number}";
                }

                return Name;
            }
        }

        public string FormattedEpisodes
        {
            get
            {
                if (WatchedEpisodes == null)
                {
                    return EpisodeOrder?.ToString() ?? "0";
                }

                return $"{WatchedEpisodes}/{EpisodeOrder}";
            }
        }
    }
}

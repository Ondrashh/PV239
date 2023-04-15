using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public partial class EpisodeModel: ObservableObject
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
        public int runtime;

        [ObservableProperty]
        public int season;

        [ObservableProperty]
        public DateTime? aired;

        [ObservableProperty]
        public double? averageRating;

        [ObservableProperty] 
        public bool watched;

        public string FormattedName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return $"Episode {Number}";
                }
                return Name;
            }
        }
    }
}

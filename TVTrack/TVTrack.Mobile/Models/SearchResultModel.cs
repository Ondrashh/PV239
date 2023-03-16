using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public partial class SearchResultModel: ObservableObject
    {
        [ObservableProperty]
        public double score;

        [ObservableProperty]
        public ShowPreviewModel show;
    }
}

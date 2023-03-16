using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public partial class ShowPreviewModel: ObservableObject
    {
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string imageURL;
    }
}

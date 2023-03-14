using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public class ShowDetailModel: ObservableObject
    {
        public string Name { get; }
        public string Summary { get; }
        public string ImageURL { get; }
    }
}

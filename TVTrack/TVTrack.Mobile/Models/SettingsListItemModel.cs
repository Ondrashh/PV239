using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models
{
    public class SettingsListItemModel
    {
        public string Route { get; set; }
        public string Name { get; set; }
        public Type ViewType { get; set; }
        public string FontIcon { get; set; }
    }
}

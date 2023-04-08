using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.ViewModels
{
    public interface IViewModel
    {
        public int ID { get; set; }
        Task OnAppearingAsync();
    }

}

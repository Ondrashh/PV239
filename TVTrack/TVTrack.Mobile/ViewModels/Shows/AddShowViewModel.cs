using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.ViewModels.Shows
{
    public partial class AddShowViewModel : ViewModelBase
    {
        public AddShowViewModel(IMapper mapper) : base(mapper)
        {
        }

        public override Task OnAppearingAsync()
        {
            return base.OnAppearingAsync();
        }
    }
}

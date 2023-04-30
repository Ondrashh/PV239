using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.ViewModels.Settings;

namespace TVTrack.Mobile.Views.Settings;

public partial class GoogleCalendarAuthView
{
    public GoogleCalendarAuthView(GoogleCalendarAuthViewModel viewModel): base(viewModel)
    {
        InitializeComponent();
    }
}
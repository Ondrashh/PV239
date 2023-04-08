using TVTrack.Mobile.ViewModels;
using TVTrack.Mobile.ViewModels.Settings;

namespace TVTrack.Mobile.Views.Settings;

public partial class SettingsListView 
{
	public SettingsListView(SettingsViewModel viewModel): base(viewModel)
	{
		InitializeComponent();
	}
}
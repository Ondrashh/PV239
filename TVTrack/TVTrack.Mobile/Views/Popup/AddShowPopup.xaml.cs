using CommunityToolkit.Maui.Views;
using TVTrack.Mobile.ViewModels.Shows;

namespace TVTrack.Mobile.Views.Popup;

public partial class AddShowPopup
{
	public AddShowPopup(AddShowViewModel viewModel): base(viewModel)
	{
		InitializeComponent();
	}
}
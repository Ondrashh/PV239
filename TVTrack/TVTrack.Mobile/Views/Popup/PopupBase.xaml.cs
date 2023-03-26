using TVTrack.Mobile.ViewModels;

namespace TVTrack.Mobile.Views.Popup;

public abstract partial class PopupBase
{
    protected IViewModel viewModel { get; }

    public PopupBase(IViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = this.viewModel = viewModel;
        this.Opened += PopupBase_Opened;
    }

    private void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        this.viewModel.OnAppearingAsync();
    }
}
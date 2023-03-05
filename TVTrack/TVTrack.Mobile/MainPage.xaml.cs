using TVTrack.Mobile.ViewModels;

namespace TVTrack.Mobile;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        InitializeComponent();

        BindingContext = new SearchViewModel();
    }
}


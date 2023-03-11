using TVTrack.Mobile.ViewModels;

namespace TVTrack.Mobile.Views;

public partial class SearchView : ContentPage
{
	public SearchView()
	{
		InitializeComponent();

        BindingContext = new SearchViewModel();
    }
}
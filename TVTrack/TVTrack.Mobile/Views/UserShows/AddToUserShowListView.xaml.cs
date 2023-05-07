using CommunityToolkit.Mvvm.Input;
using TVTrack.API.Client;
using TVTrack.Mobile.ViewModels.UserShows;

namespace TVTrack.Mobile.Views.UserShows;

public partial class AddToUserShowListView
{
    private readonly TVTrackClient _client;

    public AddToUserShowListView(AddToUserShowListViewModel viewModel, TVTrackClient client) : base(viewModel)
	{
        _client = client;
		InitializeComponent();
    }
}
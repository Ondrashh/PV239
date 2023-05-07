using CommunityToolkit.Mvvm.Input;
using TVTrack.API.Client;
using TVTrack.Mobile.ViewModels.UserShows;

namespace TVTrack.Mobile.Views.UserShows;

public partial class AddNewUserShowView
{
    private readonly TVTrackClient _client;

    public AddNewUserShowView(AddNewUserShowViewModel viewModel, TVTrackClient client) : base(viewModel)
	{
        _client = client;
		InitializeComponent();
    }

    async void OnButtonClicked(object sender, EventArgs args)
    {
        if (nameEntry.Text != null && nameEntry.Text.Length > 0)
        {
            await _client.CreateUserShow("test", nameEntry.Text, descriptionEntry.Text);
            await Shell.Current.GoToAsync("///userLists", new Dictionary<string, object>
            {
            });
        }
    }
}
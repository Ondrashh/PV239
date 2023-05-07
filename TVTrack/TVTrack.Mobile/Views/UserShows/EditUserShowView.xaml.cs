using CommunityToolkit.Mvvm.Input;
using TVTrack.API.Client;
using TVTrack.Mobile.ViewModels.UserShows;

namespace TVTrack.Mobile.Views.UserShows;

public partial class EditUserShowView
{
    private readonly TVTrackClient _client;

    public EditUserShowView(EditUserShowViewModel viewModel, TVTrackClient client) : base(viewModel)
	{
        _client = client;
		InitializeComponent();
    }

    async void OnButtonClicked(object sender, EventArgs args)
    {
        if (nameEntry.Text != null && nameEntry.Text.Length > 0)
        {
            await _client.EditUserShow("test", Int32.Parse(id.Text), nameEntry.Text, descriptionEntry.Text);
            await Shell.Current.GoToAsync("///userLists", new Dictionary<string, object>
            {
            });
        }
    }
}
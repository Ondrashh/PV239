using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;

namespace TVTrack.Mobile.ViewModels.Login
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly TVTrackClient _client;

        public RegisterViewModel(TVTrackClient client, IMapper mapper) : base(mapper)
        {
            _client = client;
            _redirectIfNotLoggedIn = false;
        }

        [ObservableProperty]
        public bool agreedToTerms = false;

        [RelayCommand]
        public async Task ShowTerms()
        {
            await Shell.Current.GoToAsync("terms");
        }

        [RelayCommand]
        public async Task Register(Entry newUsernameEntry)
        {
            // Workaround for bug in MAUI https://github.com/dotnet/maui/issues/12002 preventing
            // soft keyboard closing on Entry unfocus on Android
            newUsernameEntry.IsEnabled = false;
            newUsernameEntry.IsEnabled = true;

            var newUsername = newUsernameEntry.Text;

            if(newUsername == null || newUsername == string.Empty)
            {
                await AlertHelper.ShowErrorSnackbar("Username cannot be empty.");
                return;
            }

            if(!AgreedToTerms)
            {
                await AlertHelper.ShowErrorSnackbar("You must agree to the Terms and Conditions.");
                return;
            }

            LoadingStart();
            var result = await _client.RegisterUser(newUsername);
            LoadingEnd();
            
            if (result.Success)
            {
                await StorageHelper.StoreUsername(newUsername);
                await Shell.Current.GoToAsync("///home");
                await AlertHelper.ShowToast("Account successfully created.");
            }
            else
            {
                await AlertHelper.ShowErrorSnackbar($"Registering failed: {result.Reason}.");
            }
        }
    }
}

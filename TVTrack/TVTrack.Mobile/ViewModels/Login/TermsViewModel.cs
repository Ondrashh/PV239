using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TVTrack.Mobile.ViewModels.Login
{
    public partial class TermsViewModel : ViewModelBase
    {
        public TermsViewModel(IMapper mapper) : base(mapper)
        {
            _redirectIfNotLoggedIn = false;
        }

        [ObservableProperty]
        public string termsAndConditions = string.Empty;

        public async override Task OnAppearingAsync()
        {
            LoadingStart();
            using var stream = await FileSystem.OpenAppPackageFileAsync("termsandconditions.txt");
            using var reader = new StreamReader(stream);
            TermsAndConditions = reader.ReadToEnd();
            await base.OnAppearingAsync();
            LoadingEnd();
        }
    }
}

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Views.Login;

namespace TVTrack.Mobile.ViewModels
{
    public abstract partial class ViewModelBase : ObservableObject, IViewModel
    {
        protected readonly IMapper _mapper;
        public ViewModelBase(IMapper mapper)
        {
            _mapper = mapper;
        }

        [ObservableProperty]
        public int itemID;

        [ObservableProperty]
        public bool loadingInProgress = false;

        [ObservableProperty]
        public bool noLoadingInProgress = true;

        [ObservableProperty]
        public string loggedUsername = null;

        public virtual async Task OnAppearingAsync()
        {
            LoggedUsername = await StorageHelper.GetUsername();

            if (LoggedUsername == null)
            {
                await Shell.Current.GoToAsync("//login");
            }
        }

        [RelayCommand]
        private Task GoBackAsync()
        {
            if (Shell.Current.Navigation.NavigationStack.Count > 1)
            {
                Shell.Current.SendBackButtonPressed();
            }

            return Task.CompletedTask;
        }

        protected void LoadingStart()
        {
            LoadingInProgress = true;
            NoLoadingInProgress = false;
        }

        protected void LoadingEnd()
        {
            LoadingInProgress = false;
            NoLoadingInProgress = true;
        }
    }
}

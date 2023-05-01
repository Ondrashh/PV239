using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels
{
    public partial class MainPageViewModel : ViewModelBase
    {
        private readonly AppSettings _settings;

        public MainPageViewModel(IMapper mapper,
            IConfiguration configuration) : base(mapper)
        {
            _settings = AppSettings.Get(configuration);
        }


        [RelayCommand]
        public async Task OpenShowDetailAsync(string id)
        {
            await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
            {
                [nameof(id)] = int.Parse(id)
            });
        }
    }
}

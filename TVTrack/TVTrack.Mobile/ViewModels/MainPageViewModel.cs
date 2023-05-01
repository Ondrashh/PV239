using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [RelayCommand]
        public async Task LogOut()
        {
            await StorageHelper.LogOut();
            await AlertHelper.ShowToast("Logged out!");
            await Shell.Current.GoToAsync("///login");
        }
    }
}

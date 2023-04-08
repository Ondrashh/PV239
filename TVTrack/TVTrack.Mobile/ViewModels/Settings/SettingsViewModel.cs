using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Settings
{
    public partial class SettingsViewModel: ViewModelBase
    {
        public SettingsViewModel(IMapper mapper) : base(mapper)
        {
        }

        public ObservableCollection<SettingsListItemModel> Items { get; set; } = new();

        public override Task OnAppearingAsync()
        {
            Items.Clear();

            foreach (var item in Routes.SettingsRoutes)
            {
                Items.Add(item);
            }

            return base.OnAppearingAsync();
        }

        [RelayCommand]
        public async Task OpenSettingsItemAsync(SettingsListItemModel item)
        {
            await Shell.Current.GoToAsync(item.Route);
        }
    }
}

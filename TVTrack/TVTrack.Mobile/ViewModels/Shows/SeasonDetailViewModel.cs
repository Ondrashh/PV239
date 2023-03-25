﻿using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(Season), "season")]
    [QueryProperty(nameof(Episodes), "episodes")]
    public partial class SeasonDetailViewModel : ViewModelBase
    {
        public SeasonDetailViewModel(IMapper mapper) : base(mapper)
        {
        }

        [ObservableProperty]
        public SeasonModel season;
        [ObservableProperty]
        public List<EpisodeModel> episodes;

        public override Task OnAppearingAsync()
        {
            return base.OnAppearingAsync();
        }

        [RelayCommand]
        public async Task OpenEpisodeDetailAsync(Image chevron)
        {
            await chevron.RotateTo(180, 1000);
        }
    }
}
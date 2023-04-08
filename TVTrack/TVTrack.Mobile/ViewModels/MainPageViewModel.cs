using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Installations;
using Firebase.Messaging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.Models;
using TVTrack.Mobile.Models.Notifications;

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

    }
}

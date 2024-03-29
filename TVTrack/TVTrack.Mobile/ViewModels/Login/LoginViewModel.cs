﻿using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Login
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly TVTrackClient _client;

        [ObservableProperty]
        public ObservableCollection<UserListItemModel> users;

        public LoginViewModel(TVTrackClient client, IMapper mapper) : base(mapper)
        {
            _client = client;
            _redirectIfNotLoggedIn = false;
        }

        public override async Task OnAppearingAsync()
        {
            LoadingStart();
            
            var usersFromApi = await _client.GetUsers();
            Users = _mapper.Map<ObservableCollection<UserListItemModel>>(usersFromApi);
            await base.OnAppearingAsync();

            LoadingEnd();
        }

        [RelayCommand]
        public async Task LogIn(UserListItemModel userListItem)
        {
            if (userListItem == null)
            {
                await AlertHelper.ShowErrorSnackbar("You have to pick username to log in.");
                return;
            }
            await StorageHelper.StoreUsername(userListItem.Username);
            await Shell.Current.GoToAsync("///userLists");
            await AlertHelper.ShowToast("Logged in");

            if (Preferences.ContainsKey("DeviceToken"))
            {
                var token = Preferences.Get("DeviceToken", null);
                await _client.PutFCMToken(userListItem.username, token);
            }
        }

        [RelayCommand]
        public async Task GoToRegister()
        {
            await Shell.Current.GoToAsync("register");
        }
    }
}

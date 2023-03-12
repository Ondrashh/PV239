﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.ViewModels
{
    public abstract partial class ViewModelBase : ObservableObject, IViewModel
    {
        public virtual Task OnAppearingAsync()
        {
            return Task.CompletedTask;
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
    }
}

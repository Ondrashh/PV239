﻿using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.Views.Popup;

namespace TVTrack.Mobile.Helpers
{
    public class PopupHelper: IHelper
    {
        private readonly IServiceProvider _serviceProvider;
        public PopupHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ShowPopupAsync<T>()
            where T: PopupBase
        {
            await ShowPopupAsync<T>(0);
        }

        public async Task ShowPopupAsync<T>(int id)
            where T : PopupBase
        {
            var popup = _serviceProvider.GetRequiredService<T>();
            popup.ID = id;
            var w = DeviceDisplay.MainDisplayInfo.Width * 0.37;
            popup.Size = new Size(w, popup.Size.Height);

            await Application.Current.MainPage.ShowPopupAsync(popup);
        }

    }
}

using CommunityToolkit.Maui.Views;
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

        private static PopupBase _lastPopup;

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
            var w = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            popup.Size = new Size(w, popup.Size.Height);
            _lastPopup = popup;

            await Application.Current.MainPage.ShowPopupAsync(popup);
        }


        public static void CloseLastPopup()
        {
            if (_lastPopup != null)
            {
                _lastPopup.Close();
            }
        }
    }
}

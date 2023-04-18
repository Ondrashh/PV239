using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
#if ANDROID
using Firebase.Installations;
using Firebase.Messaging;
#endif
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TVTrack.Mobile.Models;
using TVTrack.Mobile.Models.Notifications;

namespace TVTrack.Mobile.ViewModels.Settings
{
    public partial class DeveloperToolsViewModel: ViewModelBase
    {
        private readonly AppSettings _settings;
        public DeveloperToolsViewModel(IMapper mapper, IConfiguration configuration) : base(mapper)
        {
            _settings = AppSettings.Get(configuration);
        }

        [ObservableProperty]
        public string notificationTitle;

        [ObservableProperty]
        public string notificationBody;

        [RelayCommand]
        public async Task SendNotificationAsync()
        {
            // We only support android :)
            if (!OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
            {
                return;
            }
#if ANDROID
            var FCMtoken = FirebaseMessaging.Instance.GetToken().GetResult(Java.Lang.Class.FromType(typeof(InstallationTokenResult))).ToString();

            var pushNotificationRequest = new PushNotificationRequest
            {
                Notification = new NotificationMessageBody
                {
                    Title = NotificationTitle,
                    Body = NotificationBody
                },
                Data = new Dictionary<string, string>(),
                RegistrationIDs = new List<string> { FCMtoken }
            };

            string url = "https://fcm.googleapis.com/fcm/send";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + _settings.FCMServerKey);

            string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
            var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("Not OK!");
            }

#endif
        }
    }
}

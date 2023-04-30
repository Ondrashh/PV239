using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Settings
{
    public partial class GoogleCalendarAuthViewModel: ViewModelBase
    {
        private readonly AppSettings _settings;

        public GoogleCalendarAuthViewModel(IMapper mapper, AppSettings settings) : base(mapper)
        {
            _settings = settings;
        }

        [ObservableProperty] 
        public bool authenticated;

        [RelayCommand]
        public async Task OpenAuthenticateAsync()
        {
            try
            {
                string auth_uri = _settings.Google.AuthURI;
                string client_id = _settings.Google.ClientID;
                string token_uri = _settings.Google.TokenURI;
                string redirect_uri = "com.mobile.tvtrack://";

                var authUrl = $"{auth_uri}?response_type=code" +
                              $"&redirect_uri={redirect_uri}" +
                              $"&client_id={client_id}" +
                              $"&scope=https://www.googleapis.com/auth/calendar.events" +
                              $"&include_granted_scopes=true" +
                              $"&state=state_parameter_passthrough_value";

                var response = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions()
                {
                    Url = new Uri(authUrl),
                    CallbackUrl = new Uri(redirect_uri)
                });

                var codeToken = response.Properties["code"];

                var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", client_id),
                    new KeyValuePair<string, string>("redirect_uri", redirect_uri),
                    new KeyValuePair<string, string>("code", codeToken),
                });

                HttpClient client = new HttpClient();
                var accessTokenResponse = await client.PostAsync(token_uri, parameters);

                if (accessTokenResponse.IsSuccessStatusCode)
                {
                    var data = await accessTokenResponse.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

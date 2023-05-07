using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TVTrack.API.Client;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Models;
using TVTrack.Mobile.Models.Calendar;

namespace TVTrack.Mobile.ViewModels.Settings
{
    public partial class GoogleCalendarAuthViewModel: ViewModelBase
    {
        private readonly TVTrackClient _client;
        private readonly AppSettings _settings;

        public GoogleCalendarAuthViewModel(IMapper mapper, 
            TVTrackClient client,
            AppSettings settings) : base(mapper)
        {
            _client = client;
            _settings = settings;
        }

        [ObservableProperty] 
        public bool authenticated;

        public override async Task OnAppearingAsync()
        {
            var username = await StorageHelper.GetUsername();
            var hasTokensModel = await _client.GetHasTokens(username);

            Authenticated = hasTokensModel.HasGoogleCalendar;
        }

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

                using var client = new HttpClient();
                using var accessTokenResponse = await client.PostAsync(token_uri, parameters);

                if (accessTokenResponse.IsSuccessStatusCode)
                {
                    var data = await accessTokenResponse.Content.ReadAsStringAsync();
                    var tokens = JsonConvert.DeserializeObject<LoginResponse>(data);

                    var username = await StorageHelper.GetUsername();
                    await _client.PutGoogleCalendarToken(username, tokens.AccessToken, tokens.RefreshToken);

                    Authenticated = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                Authenticated = false;
            }
        }
    }
}

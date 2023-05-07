using Newtonsoft.Json;
using System.Text;
using TVTrack.Models.API.Notifications;

namespace TvTrackServer.Services
{
    public class NotificationService
    {
        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Notify(string deviceToken, string title, string body)
        {
            await Notify(new List<string> { deviceToken }, title, body);
        }

        public async Task Notify(List<string> deviceTokens, string title, string body)
        {
            var pushNotificationRequest = new PushNotificationRequest
            {
                Notification = new NotificationMessageBody
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>(),
                RegistrationIDs = deviceTokens
            };

            using var client = new HttpClient();

            var settings = _configuration.GetSection("Settings");
            var key = settings.GetValue<string>("FCMServerKey");
            var url = settings.GetValue<string>("FCMURL");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + key);

            var serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
            var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("Not OK!");
            }
        }
    }
}

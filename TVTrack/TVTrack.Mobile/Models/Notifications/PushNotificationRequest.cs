using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Models.Notifications
{
    public class PushNotificationRequest
    {
        [JsonProperty("registration_ids")]
        public List<string> RegistrationIDs { get; set; } = new List<string>();
        [JsonProperty("notification")]
        public NotificationMessageBody Notification { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}

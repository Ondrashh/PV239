using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.API.Client
{
    internal class TVTrackEndpoints
    {
        public const string SEARCH = "shows/search";
        public const string SHOW = "shows/{id}";
        public const string NOTIFICATION = "shows/{id}/notifications";
        public const string CALENDAR = "shows/{id}/calendar";
        public const string EPISODE_WATCHED = "shows/{showId}/episodes/{epId}";
        public const string RATE = "shows/{id}/ratings";

        public const string LISTS = "showlists";
        public const string LIST_CREATE = "showlists";
        public const string LIST_UPDATE = "showlists/{id}";
        public const string LIST_ADD_NEW_SHOW = "showlists/{listId}/shows";
        public const string LIST_DELETE_SHOW = "showlists/{listId}/shows/{showId}";
        public const string LIST_DETAIL = "showlists/{id}";
        public const string LIST_DELETE = "showlists/{id}";
        public const string LIST_AVAILABLE = "showlists/available/{username}/{id}";
        public const string LIST_DEFAULT = "showlists/default/shows";
        public const string LIST_DEFAULT_DELETE = "/showlists/default/shows/{id}";
        public const string LIST_USER = "showlists";

        public const string USERS = "users";
        public const string USER_SINGLE = "users/{username}";

        public const string FCM_TOKEN = "tokens/fcm/{username}";
        public const string GCAL_TOKEN = "tokens/gc/{username}";

        public const string HAS_TOKENS = "tokens/hastokens/{username}";
    }
}

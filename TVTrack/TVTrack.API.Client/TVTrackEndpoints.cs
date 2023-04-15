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
        public const string LIST_DETAIL = "showlists/{id}";
        public const string LIST_DEFAULT = "showlists/default/shows";
        public const string LIST_DEFAULT_DELETE = "/showlists/default/shows/{id}";
        public const string LIST_USER = "showlists/{id}/shows";

        public const string USERS = "users";
        public const string USER_SINGLE = "users/{username}";
    }
}

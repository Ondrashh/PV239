using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Models.API.Responses
{
    public class UserHasTokensModel
    {
        public string Username { get; set; }
        public bool HasFCM { get; set; }
        public bool HasGoogleCalendar { get; set; }
    }
}

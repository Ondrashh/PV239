using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Models.Database;

namespace TVTrack.Models.API.Database
{
    public class Tokens
    {
        [Key]
        public int Id { get; set; }
        [InverseProperty(nameof(Models.Database.User.Id))]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string? FCMDeviceToken { get; set; }
        public string? GoogleCalendarToken { get; set; }
        public string? GoogleCalendarRefreshToken { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVTrack.Models.Database
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }

        [InverseProperty(nameof(ShowList.User))]
        public List<ShowList> ShowLists = new();

        [InverseProperty(nameof(ShowActivity.User))]
        public List<ShowActivity> ShowActivities = new();
    }
}

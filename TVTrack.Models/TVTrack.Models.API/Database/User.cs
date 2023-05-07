using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TVTrack.Models.API.Database;

namespace TVTrack.Models.Database
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }

        [ForeignKey(nameof(Tokens))]
        public int TokensId { get; set; }
        public virtual Tokens Tokens { get; set; } = new();

        [InverseProperty(nameof(ShowList.User))]
        public List<ShowList> ShowLists = new();

        [InverseProperty(nameof(ShowActivity.User))]
        public List<ShowActivity> ShowActivities = new();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVTrack.Models.Database
{
    /// <summary>
    /// Entity holding TvMazeId identification for listing purposes.
    /// </summary>
    public class ShowListItem
    {
        [Key]
        public int Id { get; set; }
        public int TvMazeId { get; set; }

        [ForeignKey(nameof(ShowList))]
        public int ShowListId { get; set; }
        public virtual ShowList ShowList { get; set; } = null!;
    }
}

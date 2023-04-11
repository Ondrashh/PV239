using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvTrackServer.Models.Database;

public class ShowList
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Default { get; set; } = false;

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [InverseProperty(nameof(ShowListItem.ShowList))]
    public List<ShowListItem> Shows { get; set; } = new();
}

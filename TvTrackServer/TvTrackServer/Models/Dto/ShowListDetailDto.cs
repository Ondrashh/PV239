using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TVTrack.Models.Database;

namespace TvTrackServer.Models.Dto
{
    public class ShowListDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Default { get; set; } = false;
        public User User { get; set; } = null!;
        public List<ShowPreviewDto> Shows { get; set; } = new();
    }
}

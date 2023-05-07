
namespace TVTrack.API.Models
{
    public class ShowListPreview
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Default { get; set; } = false;
        public int ShowCount { get; set; }
    }
}

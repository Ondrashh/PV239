
namespace TvTrackServer.Models.Dto
{
    public class RatingDto
    {
        public double? Average { get; set; }

        public double? TvTrackRating { get; set; } = null;
        public int TvTrackRatingCount { get; set; } = 0;
    }
}

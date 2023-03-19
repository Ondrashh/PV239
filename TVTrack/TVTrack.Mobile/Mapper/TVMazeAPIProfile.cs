using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.TVMaze.Client.Models;
using TVTrack.Mobile.Models;
using System.Globalization;

namespace TVTrack.Mobile.Mapper
{
    public class TVMazeAPIProfile: Profile
    {
        public TVMazeAPIProfile()
        {
            CreateMap<Show, ShowDetailModel>()
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.Seasons, opt => opt.MapFrom(src => src.Embedded.Seasons))
                .ForMember(x => x.Episodes, opt => opt.MapFrom(src => src.Embedded.Episodes))
                .ForMember(x => x.Premiered, opt => opt.MapFrom(src => DateTime.Parse(src.Premiered)))
                .ForMember(x => x.Ended, opt => opt.MapFrom(src => DateTime.Parse(src.Ended)))
                .ForMember(x => x.AverageRating, opt => opt.MapFrom(src => src.Rating.Average))
                .AfterMap((src, dest) => dest.Network = src.WebChannel != null ? src.WebChannel.Name : (src.Network != null ? src.Network.Name : "<i>Unknown</i>"));

            CreateMap<Show, ShowPreviewModel>()
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original));

            CreateMap<Season, SeasonModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.FormattedName, opt => opt.Ignore())
                .ForMember(x => x.Premiered, opt => opt.MapFrom(src => DateTime.Parse(src.PremiereDate)))
                .ForMember(x => x.Ended, opt => opt.MapFrom(src => DateTime.Parse(src.EndDate)));

            CreateMap<Episode, EpisodeModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.FormattedName, opt => opt.Ignore());
        }
    }
}

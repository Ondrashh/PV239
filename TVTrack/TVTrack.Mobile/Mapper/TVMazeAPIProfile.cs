using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.TVMaze.Client.Models;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.Mapper
{
    public class TVMazeAPIProfile: Profile
    {
        public TVMazeAPIProfile()
        {
            CreateMap<Show, ShowDetailModel>()
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.Seasons, opt => opt.MapFrom(src => src.Embedded.Seasons))
                .ForMember(x => x.Episodes, opt => opt.MapFrom(src => src.Embedded.Episodes));

            CreateMap<Show, ShowPreviewModel>()
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original));

            CreateMap<Season, SeasonModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.FormattedName, opt => opt.Ignore());

            CreateMap<Episode, EpisodeModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.FormattedName, opt => opt.Ignore());
        }
    }
}

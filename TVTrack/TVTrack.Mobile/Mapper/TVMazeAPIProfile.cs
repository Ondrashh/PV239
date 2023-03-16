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
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original));

            CreateMap<Show, ShowPreviewModel>()
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original));
        }
    }
}

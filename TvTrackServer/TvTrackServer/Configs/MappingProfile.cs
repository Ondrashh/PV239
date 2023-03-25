using AutoMapper;
using TvTrackServer.Models.Database;
using TvTrackServer.Models.Dto;

namespace TvTrackServer.Configs
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<CreateShowListDto, ShowList>();
            CreateMap<ShowList, ShowListPreviewDto>()
                .ForMember(sl => sl.ShowCount, opt => opt.MapFrom(src => src.Shows.Count));
        }
    }
}

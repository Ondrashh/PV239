using AutoMapper;
using TVTrack.Models.Database;
using TVTrack.Models.TvMaze;
using TvTrackServer.Models.Dto;

namespace TvTrackServer.Configs;

public class MappingProfile : Profile
{
    public MappingProfile() {
        CreateMap<CreateShowListDto, ShowList>();
        CreateMap<Show, ShowListItem>();
        CreateMap<ShowList, ShowListPreviewDto>()
            .ForMember(sl => sl.ShowCount, opt => opt.MapFrom(src => src.Shows.Count));
        CreateMap<ShowList, ShowListDetailDto>();
        CreateMap<ShowListItem, ShowListItemPreviewDto>();
        CreateMap<ShowListItem, ShowPreviewDto>();
        CreateMap<ShowList, ShowListAvailableDto>();
        CreateMap<Show, ShowPreviewDto>();
    }
}

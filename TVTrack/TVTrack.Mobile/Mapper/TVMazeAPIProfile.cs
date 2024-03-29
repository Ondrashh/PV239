﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.Models;
using System.Globalization;
using TVTrack.Models.TvMaze;
using TVTrack.Models.Database;
using TVTrack.API.Models;
using TVTrack.API.Client.Models;

namespace TVTrack.Mobile.Mapper
{
    public class TVMazeAPIProfile: Profile
    {
        public TVMazeAPIProfile()
        {
            CreateMap<ShowListPreview, ShowListPreviewModel>();
            CreateMap<ShowListDetail, UserShowsDetailModel>()
                .ForMember(x => x.IsDefault, opt => opt.MapFrom(src => src.Default));
            CreateMap<Show, ShowDetailModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.Seasons, opt => opt.MapFrom(src => src.Embedded.Seasons))
                .ForMember(x => x.Episodes, opt => opt.MapFrom(src => src.Embedded.Episodes))
                .ForMember(x => x.Premiered, opt => opt.MapFrom(src => DateTime.Parse(src.Premiered)))
                .ForMember(x => x.Ended, opt => opt.MapFrom(src => DateTime.Parse(src.Ended)))
                .ForMember(x => x.AverageRating, opt => opt.MapFrom(src => src.Rating.Average))
                .AfterMap((src, dest) => dest.Network = src.WebChannel != null
                    ? src.WebChannel.Name
                    : (src.Network != null ? src.Network.Name : "<i>Unknown</i>"))
                .AfterMap((src, dest) => dest.Schedule = src.Schedule != null && src.Schedule.Days.Any()
                    ? $"{src.Schedule.Days.FirstOrDefault()} at {src.Schedule.Time ?? ""}"
                    : "<i>Unknown</i>")
                .AfterMap((src, dest) =>
                {
                    foreach (var season in dest.Seasons)
                    {
                        season.WatchedEpisodes = dest.Episodes?.Count(x => x.Watched && x.Season == season.Number) ?? 0;
                    }
                });

            CreateMap<Show, ShowPreviewModel>()
                .ForMember(x => x.ImageURL, opt => opt.MapFrom(src => src.Image.Original));

            CreateMap<Season, SeasonModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.FormattedName, opt => opt.Ignore())
                .ForMember(x => x.Premiered, opt => opt.MapFrom(src => DateTime.Parse(src.PremiereDate)))
                .ForMember(x => x.Ended, opt => opt.MapFrom(src => DateTime.Parse(src.EndDate)))
                .AfterMap((src, dest) => dest.Network = src.WebChannel != null ? src.WebChannel.Name : ( src.Network != null ? src.Network.Name : "<i>Unknown</i>" ));

            CreateMap<Episode, EpisodeModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(src => src.Image.Original))
                .ForMember(x => x.Aired, opt => opt.MapFrom(src => src.Airstamp))
                .ForMember(x => x.AverageRating, opt => opt.MapFrom(src => src.Rating.Average))
                .ForMember(x => x.FormattedName, opt => opt.Ignore())
                .ForMember(x => x.Watched, opt => opt.MapFrom(src => src.UserWatched));

            CreateMap<User, UserListItemModel>()
                .ForMember(x => x.Username, opt => opt.MapFrom(src => src.Username));
        }
    }
}

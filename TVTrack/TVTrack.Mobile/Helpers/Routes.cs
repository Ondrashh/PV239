﻿using TVTrack.Mobile.Models;
using TVTrack.Mobile.Resources.Fonts;
using TVTrack.Mobile.Views.Settings;
using TVTrack.Mobile.Views.Shows;
using TVTrack.Mobile.Views.UserShows;

namespace TVTrack.Mobile.Helpers
{
    public static class Routes
    {
        public static List<SettingsListItemModel> SettingsRoutes = new()
        {
            new()
            {
                Name = "Developer Tools", 
                FontIcon = FontAwesomeIcons.Wrench, 
                Route = "///settings/developer",
                ViewType = typeof(DeveloperToolsView)
            }
        };

        public static MauiAppBuilder ConfigureRoutes(this MauiAppBuilder builder)
        {
            SettingsRoutes.ForEach(x => Routing.RegisterRoute(x.Route, x.ViewType));

            Routing.RegisterRoute("///userLists/detail", typeof(UserShowDetailView));
            Routing.RegisterRoute("///userLists/detail/edit", typeof(EditUserShowView));
            Routing.RegisterRoute("///userLists/new", typeof(AddNewUserShowView));
            Routing.RegisterRoute("///userLists/detail/show", typeof(ShowDetailView));
            Routing.RegisterRoute("///userLists/detail/show/add", typeof(AddToUserShowListView));

            Routing.RegisterRoute("///search/detail", typeof(ShowDetailView));
            Routing.RegisterRoute("///search/detail/season", typeof(SeasonDetailView));
            Routing.RegisterRoute("///search/detail/add", typeof(AddToUserShowListView));

            return builder;
        }
    }
}

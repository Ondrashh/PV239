using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.Models;
using TVTrack.Mobile.Resources.Fonts;
using TVTrack.Mobile.Views.Settings;
using TVTrack.Mobile.Views.Shows;

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
            },
            new()
            {
                Name = "Google Calendar",
                FontIcon = FontAwesomeIcons.Calendar,
                Route = "///settings/googlecal",
                ViewType = typeof(GoogleCalendarAuthView)
            }
        };

        public static MauiAppBuilder ConfigureRoutes(this MauiAppBuilder builder)
        {
            SettingsRoutes.ForEach(x => Routing.RegisterRoute(x.Route, x.ViewType));

            Routing.RegisterRoute("///home/detail", typeof(ShowDetailView));
            Routing.RegisterRoute("///home/detail/season", typeof(SeasonDetailView));

            Routing.RegisterRoute("///search/detail", typeof(ShowDetailView));
            Routing.RegisterRoute("///search/detail/season", typeof(SeasonDetailView));

            Routing.RegisterRoute("///login", typeof(LoginView));

            return builder;
        }
    }
}

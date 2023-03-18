using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using TVTrack.Mobile.Fonts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;
using TVTrack.Mobile.ViewModels;
using TVTrack.Mobile.Views;
using TVTrack.Mobile.Views.Shows;
using TVTrack.Mobile.Mapper;
using TVTrack.TVMaze.Client;
using TVTrack.TVMaze.Client.Models;

namespace TVTrack.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("fa-solid-900.ttf", Fonts.Fonts.FontAwesome);
			});

		RegisterAPI(builder.Services);

        RegisterViews(builder.Services);
		RegisterViewModels(builder.Services);

		RegisterRoutes();

		builder.Services.AddAutoMapper(typeof(TVMazeAPIProfile));

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

    private static void RegisterRoutes()
    {
		//Routing.RegisterRoute("///search", typeof(SearchView));
        Routing.RegisterRoute("///search/detail", typeof(ShowDetailView));
        Routing.RegisterRoute("///search/detail/season", typeof(SeasonDetailView));
    }

    private static void RegisterViewModels(IServiceCollection services)
	{
        services.Scan(selector => selector
			.FromAssemblyOf<App>()
			.AddClasses(filter => filter.AssignableTo<IViewModel>())
			.AsSelfWithInterfaces()
			.WithTransientLifetime());
    }

    private static void RegisterViews(IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<ContentPageBase>())
            .AsSelf()
            .WithTransientLifetime());
    }

	private static void RegisterAPI(IServiceCollection services)
	{
		services.AddSingleton(typeof(TVMazeClient));
	}
}

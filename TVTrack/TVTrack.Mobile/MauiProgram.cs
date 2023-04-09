using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;
using TVTrack.Mobile.ViewModels;
using TVTrack.Mobile.Views;
using TVTrack.Mobile.Views.Shows;
using TVTrack.Mobile.Mapper;
using TVTrack.TVMaze.Client;
using TVTrack.TVMaze.Client.Models;
using TVTrack.Mobile.Views.Popup;
using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Resources.Fonts;

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
                fonts.AddFont("fa-solid-900.ttf", Fonts.FontAwesome);
            })
            .ConfigureAppSettings()
            .ConfigureHelpers()
            .ConfigureAPI()
            .ConfigureViews()
            .ConfigureViewModels()
            .RegisterRoutes();

        builder.Services.AddAutoMapper(typeof(TVMazeAPIProfile));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder ConfigureAppSettings(this MauiAppBuilder builder)
    {
        var configurationBuilder = new ConfigurationBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        var appSettingsFilePath = "TVTrack.Mobile.appsettings.json";
        using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
        if (appSettingsStream is not null)
        {
            configurationBuilder.AddJsonStream(appSettingsStream);
        }

        var configuration = configurationBuilder.Build();
        builder.Configuration.AddConfiguration(configuration);
        return builder;
    }

    private static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.Scan(selector => selector
             .FromAssemblyOf<App>()
             .AddClasses(filter => filter.AssignableTo<IViewModel>())
             .AsSelfWithInterfaces()
             .WithTransientLifetime());
        return builder;
    }

    private static MauiAppBuilder ConfigureViews(this MauiAppBuilder builder)
    {
        builder.Services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<IHelper>())
            .AsSelf()
            .WithTransientLifetime());
        return builder;
    }

    private static MauiAppBuilder ConfigureHelpers(this MauiAppBuilder builder)
    {
        builder.Services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<ContentPageBase>())
            .AddClasses(filter => filter.AssignableTo<PopupBase>())
            .AsSelf()
            .WithTransientLifetime());
        return builder;
    }

    private static MauiAppBuilder ConfigureAPI(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(typeof(TVMazeClient));
        return builder;
    }
}

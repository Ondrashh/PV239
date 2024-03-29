﻿using Microsoft.Extensions.DependencyInjection;
using TVTrack.Mobile.Services.Interfaces;

namespace TVTrack.Mobile;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
	{
        _serviceProvider = serviceProvider;
        InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnStart()
    {
        base.OnStart();

        var globalExceptionServiceInitializer = _serviceProvider.GetRequiredService<IGlobalExceptionServiceInitializer>();
        globalExceptionServiceInitializer.Initialize();
    }
}

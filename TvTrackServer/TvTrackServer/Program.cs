using Microsoft.EntityFrameworkCore;
using Quartz;
using RestSharp;
using TvTrackServer;
using TvTrackServer.Jobs;
using TvTrackServer.Models;
using TvTrackServer.Services;
using TvTrackServer.TvMazeConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TvTrackServerDbContext>(opt =>
{
    if (builder.Configuration.GetConnectionString(Environment.MachineName) == null)
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLEXPRESSConnectionSTring"));
    }
    else
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString(Environment.MachineName));
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddScoped(typeof(TvTrackServerDbContext), typeof(TvTrackServerDbContext));
builder.Services.AddTransient<NotificationService>();
builder.Services.AddTransient<TVGoogleCalendarService>();
builder.Services.AddTransient<NotificationJob>();
builder.Services.AddTransient<GoogleCalendarSyncJob>();
builder.Services.AddSingleton(typeof(TvMazeClient), (provider) =>
{
    return new TvMazeClient(new RestClient(TvMazeEndpoints.URL));
});

builder.Services.AddAutoMapper(typeof(TvTrackServer.Configs.MappingProfile));

builder.Services.Configure<QuartzOptions>(builder.Configuration.GetSection("Quartz"));

// if you are using persistent job store, you might want to alter some options
builder.Services.Configure<QuartzOptions>(options =>
{
    options.Scheduling.IgnoreDuplicates = true; // default: false
    options.Scheduling.OverWriteExistingData = true; // default: true
});
builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "TVTrack-Scheduler";
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
    q.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 10;
    });

    // TODO setup proper intervals for jobs
    q.ScheduleJob<NotificationJob>(trigger => trigger
        .WithIdentity("Daily Notification")
        .StartNow()
        .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()));

    q.ScheduleJob<GoogleCalendarSyncJob>(trigger => trigger
        .WithIdentity("Daily Calendar Synchronization")
        .StartNow()
        .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()));
});

builder.Services.AddQuartzHostedService(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

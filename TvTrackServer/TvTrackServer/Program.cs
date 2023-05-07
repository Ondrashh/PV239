using Microsoft.EntityFrameworkCore;
using TvTrackServer;

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

builder.Services.AddAutoMapper(typeof(TvTrackServer.Configs.MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

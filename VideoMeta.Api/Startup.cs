using Microsoft.EntityFrameworkCore;
using VideoMeta.Api;
using VideoMeta.Api.Services;
using VideoMeta.Data;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


builder.Services.AddDbContextPool<VideoMetaDbContext>(options => options.UseCosmos(
    config.GetValue<string>("CosmosDb:EndpointUri"),
    config.GetValue<string>("CosmosDb:PrimaryKey"),
    "VideoMetas"
));
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IVideoMetaService, VideoMetaService>();
builder.Services.AddTransient<IThemeService, ThemeService>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.StartDbSeeding();
}


app.UseHealthChecks("/healthz");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

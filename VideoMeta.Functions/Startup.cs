using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VideoMeta.Data;
using VideoMeta.Functions.Services;


[assembly: FunctionsStartup(typeof(VideoMeta.Functions.Startup))]
namespace VideoMeta.Functions;

class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddDbContextPool<VideoMetaDbContext>(options => options.UseCosmos(
            Environment.GetEnvironmentVariable("CosmosDbEndpointUri"),
            Environment.GetEnvironmentVariable("CosmosDbPrimaryKey"),
            "VideoMetas"
        ));
        
        builder.Services.AddTransient<IThemeService, ThemeService>();
        builder.Services.AddTransient<IVideoMetaService, VideoMetaService>();
    }
}

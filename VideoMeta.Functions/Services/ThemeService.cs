namespace VideoMeta.Functions.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VideoMeta.Data;
using VideoMeta.Functions.Dtos;


public interface IThemeService
{
    Task HandleEvent(ThemeEvent evt);
}


public class ThemeService : IThemeService
{
    private readonly VideoMetaDbContext videoMetaDbContext;
    private readonly ILogger<ThemeService> logger;

    public ThemeService(VideoMetaDbContext videoMetaDbContext, ILogger<ThemeService> logger)
    {
        this.videoMetaDbContext = videoMetaDbContext;
        this.logger = logger;
    }


    public async Task HandleEvent(ThemeEvent evt)
    {
        switch (evt.Type)
        {
            case ThemeEventType.CREATED:
            case ThemeEventType.UPDATED:
                await UpdateOrCreateTheme(evt);
                break;

            case ThemeEventType.DELETED:
                await SoftDeleteTheme(evt);
                break;

            case ThemeEventType.RECOVER:
                await RecoverSoftDeleteTheme(evt);
                break;

            default:
                var msg = $"Not supported theme event type: {evt.Type}";
                logger.LogError(msg);
                throw new Exception(msg);
        }
    }


    private async Task SoftDeleteTheme(ThemeEvent evt)
    {
        var theme = await videoMetaDbContext.Themes.FirstOrDefaultAsync(x => x.Id == evt.Id);
        if (theme != null)
        {
            theme.IsDeleted = true;
            theme.UpdatedTime = DateTime.UtcNow;

            await videoMetaDbContext.SaveChangesAsync();
        }
    }

    private async Task RecoverSoftDeleteTheme(ThemeEvent evt)
    {
        var theme = await videoMetaDbContext.Themes.FirstOrDefaultAsync(x => x.Id == evt.Id);
        if (theme != null)
        {
            theme.IsDeleted = false;
            theme.UpdatedTime = DateTime.UtcNow;

            await videoMetaDbContext.SaveChangesAsync();
        }
        else
        {
            logger.LogWarning("Revocer failed because theme ({}) is not found", evt.Id);
        }
    }

    private async Task UpdateOrCreateTheme(ThemeEvent evt)
    {
        var theme = await videoMetaDbContext.Themes.FirstOrDefaultAsync(x => x.Id == evt.Id);

        if (theme == null)
        {
            theme = new Theme
            {
                Id = evt.Id
            };
            videoMetaDbContext.Themes.Add(theme);
        }

        theme.Name = evt.Name;
        theme.UpdatedTime = DateTime.UtcNow;

        await videoMetaDbContext.SaveChangesAsync();
    }
}

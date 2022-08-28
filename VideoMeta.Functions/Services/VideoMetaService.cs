namespace VideoMeta.Functions.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing.Printing;
using VideoMeta.Data;
using VideoMeta.Functions.Dtos;
using Z.EntityFramework.Plus;


public interface IVideoMetaService
{
    Task ArchieveData();
    Task HandleEvent(VideoMetaEvent evt);
}


public class VideoMetaService : IVideoMetaService
{
    private readonly VideoMetaDbContext videoMetaDbContext;
    private readonly ILogger<VideoMetaService> logger;

    public VideoMetaService(VideoMetaDbContext videoMetaDbContext, ILogger<VideoMetaService> logger)
    {
        this.videoMetaDbContext = videoMetaDbContext;
        this.logger = logger;
    }


    public async Task ArchieveData()
    {
        if (!TimeSpan.TryParse(Environment.GetEnvironmentVariable("ArchiveThreshold"), out var archiveThreshold))
        {
            archiveThreshold = TimeSpan.FromDays(30);
        }
        var targetDate = DateTime.Now - archiveThreshold;

        var page = 0;
        var pageSize = 50;
        var query = videoMetaDbContext.VideoMetas.Where(x => x.IsDeleted && x.UpdatedTime < targetDate);
        var totalCount = await query.CountAsync();

        while (page * pageSize < totalCount)
        {
            logger.LogInformation("Hard delete video-meta which is soft deleted before {}, page {}", targetDate, page);
            var pagedItems = await query.OrderBy(x => x.UpdatedTime).Skip(page * pageSize).Take(pageSize).ToListAsync();
            videoMetaDbContext.VideoMetas.RemoveRange(pagedItems);
            await videoMetaDbContext.SaveChangesAsync();

            page++;
        }
    }


    public async Task HandleEvent(VideoMetaEvent evt)
    {
        switch (evt.Type)
        {
            case VideoMetaEventType.CREATED:
            case VideoMetaEventType.UPDATED:
                await CreateOrUpdateVideoMeta(evt);
                break;

            case VideoMetaEventType.DELETED:
                await SoftDeleteVideoMeta(evt);
                break;

            case VideoMetaEventType.PROCESSED:
                await SetVideoReady(evt);
                break;

            default:
                var msg = $"Not supported video-meta event type: {evt.Type}";
                logger.LogError(msg);
                throw new Exception(msg);
        }
    }


    private async Task<VideoMeta> GetOrCreateVideoMeta(VideoMetaEvent evt)
    {
        var meta = await videoMetaDbContext.VideoMetas.FirstOrDefaultAsync(x => x.Id == evt.Id);

        if (meta == null)
        {
            meta = new VideoMeta
            {
                Id = evt.Id
            };
            videoMetaDbContext.VideoMetas.Add(meta);
        }

        return meta;
    }

    private async Task SoftDeleteVideoMeta(VideoMetaEvent evt)
    {
        var meta = videoMetaDbContext.VideoMetas.FirstOrDefault(x => x.Id == evt.Id);

        if (meta != null)
        {
            meta.IsDeleted = true;
            await videoMetaDbContext.SaveChangesAsync();
        }
    }


    private async Task CreateOrUpdateVideoMeta(VideoMetaEvent evt)
    {
        var meta = await GetOrCreateVideoMeta(evt);

        if (evt.ThemeId.HasValue)
        {
            meta.ThemeId = evt.ThemeId.Value;
        }

        meta.Url = evt.Url;
        meta.Title = evt.Title;
        meta.Description = evt.Description;
        meta.IsDeleted = false;
        meta.UpdatedTime = DateTime.Now;
        await videoMetaDbContext.SaveChangesAsync();
    }

    private async Task SetVideoReady(VideoMetaEvent evt)
    {
        var meta = await GetOrCreateVideoMeta(evt);
        meta.UpdatedTime = DateTime.Now;
        meta.IsReady = true;
        await videoMetaDbContext.SaveChangesAsync();
    }
}

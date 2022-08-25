namespace VideoMeta.Api.Services;

using Microsoft.EntityFrameworkCore;
using VideoMeta.Api.Dtos;
using VideoMeta.Data;


public interface IVideoMetaService
{
    Task<VideoMetaItem?> GetVideoMeta(Guid id);
    Task<VideoMetaResp> GetVideoMetas(VideoMetaReq req);
}


public class VideoMetaService : IVideoMetaService
{
    private readonly VideoMetaDbContext db;

    public VideoMetaService(VideoMetaDbContext db)
    {
        this.db = db;
    }

    public async Task<VideoMetaItem?> GetVideoMeta(Guid id)
    {
        var meta = await db.VideoMetas.AsNoTracking()
            .Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();

        return meta == null ? null : new VideoMetaItem(meta);
    }


    public async Task<VideoMetaResp> GetVideoMetas(VideoMetaReq req)
    {
        if (req.ThemeId != null)
        {
            var theme = await db.Themes.AsNoTracking()
                .Where(x => x.Id == req.ThemeId)
                .Select(x => new { x.IsDeleted })
                .FirstOrDefaultAsync();

            if (theme == null || theme.IsDeleted)
            {
                return new VideoMetaResp(
                    PageSize: req.PageSize,
                    Page: req.Page,
                    TotalCount: 0,
                    Items: new List<VideoMetaItem>()
                );
            }
        }

        var query = db.VideoMetas.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Where(x => req.Title == null || x.Title.ToLower().Contains(req.Title.ToLower()))
            .Where(x => req.Description == null || x.Description.ToLower().Contains(req.Description.ToLower()))
            .Where(x => req.ThemeId == null || x.ThemeId == req.ThemeId);

        var totalCount = await query.LongCountAsync();

        if (totalCount > 0)
        {
            var items = await query
                .OrderBy(x => x.CreatedTime)
                .Skip(req.PageSize * (int)req.Page)
                .Take(req.PageSize)
                .Select(x => new VideoMetaItem(x))
                .ToListAsync();

            return new (
                PageSize: req.PageSize,
                Page: req.Page,
                TotalCount: totalCount,
                Items: items
            );
        }

        return new (
            PageSize: req.PageSize,
            Page: req.Page,
            TotalCount: totalCount,
            Items: new List<VideoMetaItem>()
        );
    }
}

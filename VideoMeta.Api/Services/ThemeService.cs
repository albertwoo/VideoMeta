namespace VideoMeta.Api.Services;

using Microsoft.EntityFrameworkCore;
using VideoMeta.Api.Dtos;
using VideoMeta.Data;


public interface IThemeService
{
    Task<ThemeItem?> GetTheme(Guid id);
    Task<ThemeResp> GetThemes(ThemeReq req);
}


public class ThemeService : IThemeService
{
    private readonly VideoMetaDbContext db;

    public ThemeService(VideoMetaDbContext db)
    {
        this.db = db;
    }


    public async Task<ThemeItem?> GetTheme(Guid id)
    {
        var theme = await db.Themes.AsNoTracking()
            .Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();

        return theme == null ? null : new ThemeItem(theme);
    }


    public async Task<ThemeResp> GetThemes(ThemeReq req)
    {
        var query = db.Themes.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Where(x => req.Name == null || x.Name.ToLower().Contains(req.Name.ToLower()));

        var totalCount = await query.LongCountAsync();

        if (totalCount > 0)
        {
            var items = await query
                .OrderBy(x => x.CreatedTime)
                .Skip(req.PageSize * (int)req.Page)
                .Take(req.PageSize)
                .Select(x => new ThemeItem(x))
                .ToListAsync();

            return new(
                PageSize: req.PageSize,
                Page: req.Page,
                TotalCount: totalCount,
                Items: items
            );
        }

        return new(
            PageSize: req.PageSize,
            Page: req.Page,
            TotalCount: totalCount,
            Items: new List<ThemeItem>()
        );
    }
}

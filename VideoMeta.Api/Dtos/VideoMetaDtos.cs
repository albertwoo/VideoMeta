namespace VideoMeta.Api.Dtos;

using System.ComponentModel.DataAnnotations;


public record VideoMetaReq(
    string? Title,
    string? Description,
    Guid? ThemeId,
    [Range(0, 100)] int PageSize = 10,
    [Range(0, int.MaxValue)] long Page = 0
);


public record VideoMetaResp(
    int PageSize,
    long Page,
    long TotalCount,
    IEnumerable<VideoMetaItem> Items
);


public record VideoMetaItem(
    Guid Id,
    string Title,
    string Description,
    bool IsReady,
    Guid ThemeId,
    DateTime CreatedTime,
    DateTime UpdatedTime
)
{
    public VideoMetaItem(Data.VideoMeta x) : this(
        x.Id,
        x.Title,
        x.Description,
        x.IsReady,
        x.ThemeId,
        x.CreatedTime,
        x.UpdatedTime
    )
    { }
}

namespace VideoMeta.Api.Dtos;

using System.ComponentModel.DataAnnotations;


public record ThemeReq(
    string? Name,
    [Range(0, 100)] int PageSize = 10,
    [Range(0, int.MaxValue)] long Page = 0
);

public record ThemeResp(
    int PageSize,
    long Page,
    long TotalCount,
    IEnumerable<ThemeItem> Items
);


public record ThemeItem(
    Guid Id,
    string Name,
    DateTime CreatedTime,
    DateTime UpdatedTime
)
{
    public ThemeItem(Data.Theme theme) : this(
        theme.Id,
        theme.Name,
        theme.CreatedTime,
        theme.UpdatedTime
    )
    { }
}

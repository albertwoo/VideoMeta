namespace VideoMeta.Functions.Dtos;

public record ThemeEvent(
    Guid Id,
    ThemeEventType Type,
    string? Name
);


public enum ThemeEventType
{
    CREATED,
    UPDATED,
    DELETED,
}
